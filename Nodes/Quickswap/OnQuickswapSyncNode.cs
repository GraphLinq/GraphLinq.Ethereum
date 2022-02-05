using Nethereum.Contracts;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.Polygon;
using System;
using System.Collections.Generic;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.Quickswap.Entities.QuickswapPair;

namespace NodeBlock.Plugin.Ethereum.Nodes.Quickswap
{
    [NodeDefinition("OnQuickswapSyncNode", "On Quickswap Sync", NodeTypeEnum.Event, "Quickswap")]
    [NodeGraphDescription("Event that occurs on liquidity providers update on a specific contract address, returning the reserve of both token left in the pool")]
    [NodeIDEParameters(Hidden = false)]
    public class OnQuickswapSyncNode : Node, IEventEthereumNode
    {
        public OnQuickswapSyncNode(string id, BlockGraph graph)
          : base(id, graph, typeof(OnQuickswapSyncNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(string), true));
            this.InParameters.Add("contractAddress", new NodeParameter(this, "contractAddress", typeof(string), true));

            this.OutParameters.Add("reserve0", new NodeParameter(this, "reserve0", typeof(string), false));
            this.OutParameters.Add("reserve1", new NodeParameter(this, "reserve1", typeof(string), false));
        }

        private string contractAddress = "";
        private CustomUniswapSyncEvent ethLogsSubscription;

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            if (this.InParameters["contractAddress"].GetValue() != null)
                this.contractAddress = this.InParameters["contractAddress"].GetValue().ToString();
            PolygonConnectorNode ethConnection = this.InParameters["connection"].GetValue() as PolygonConnectorNode;
            if (ethConnection.UseManaged)
            {
                ethLogsSubscription = Plugin.EventsManagerEth.NewEventTypeUniswapSync(this);
                return;
            }

            var filterTransfers = Event<SyncEventDTOBase>.GetEventABI().CreateFilterInput();
            this.ethLogsSubscription = new CustomUniswapSyncEvent(ethConnection.SocketClient);
            ethLogsSubscription.SubscriptionDataResponse += OnEventNode;
            ethLogsSubscription.SubscribeAsync(filterTransfers).Wait();
        }

        public override void OnStop()
        {
            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            if (ethConnection.UseManaged)
            {
                string eventType = ethLogsSubscription.GetType().ToString();
                Plugin.EventsManagerEth.RemoveEventNode(eventType, this);
                return;
            }
            this.ethLogsSubscription.UnsubscribeAsync().Wait();
        }

        public void OnEventNode(object sender, dynamic e)
        {
            StreamingEventArgs<Nethereum.RPC.Eth.DTOs.FilterLog> eventData = e;
            if (eventData.Response == null) return;
            var decoded = Event<SyncEventDTOBase>.DecodeEvent(e.Response);
            if (decoded == null) return;
            if (!string.IsNullOrEmpty(contractAddress) && eventData.Response.Address.ToLower() != contractAddress.ToLower()) return;
            var instanciatedParameters = this.InstanciateParametersForCycle();

            instanciatedParameters["reserve0"].SetValue(Web3.Convert.FromWei(decoded.Event.Reserve0));
            instanciatedParameters["reserve1"].SetValue(Web3.Convert.FromWei(decoded.Event.Reserve1));
            this.Graph.AddCycle(this, instanciatedParameters);
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
