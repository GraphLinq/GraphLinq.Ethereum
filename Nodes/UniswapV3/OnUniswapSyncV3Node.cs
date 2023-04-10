using Nethereum.Contracts;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.UniswapV3.Entities.UniswapPairV3;

namespace NodeBlock.Plugin.Ethereum.Nodes.UniswapV3
{
    [NodeDefinition("OnUniswapSyncV3Node", "On Uniswap v3 Sync", NodeTypeEnum.Event, "Uniswap v3")]
    [NodeGraphDescription("Event that occurs on v3 liquidity providers update on a specific contract address, returning the reserve of both token left in the pool")]
    public class OnUniswapSyncV3Node : Node, IEventEthereumNode
    {
        public OnUniswapSyncV3Node(string id, BlockGraph graph)
          : base(id, graph, typeof(OnUniswapSyncV3Node).Name)
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

            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            if (ethConnection.UseManaged)
            {
                ethLogsSubscription = Plugin.EventsManagerEth.NewEventTypeUniswapSync(this);
                return;
            }

            var filterTransfers = Event<SyncEventDTOBaseV3>.GetEventABI().CreateFilterInput();
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
            var decoded = Event<SyncEventDTOBaseV3>.DecodeEvent(e.Response);
            if (decoded == null) return;
            if (!string.IsNullOrEmpty(contractAddress) && eventData.Response.Address.ToLower() != contractAddress.ToLower()) return;
            var instanciatedParameters = this.InstanciatedParametersForCycle();

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
