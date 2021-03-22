using Nethereum.Contracts;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.BSC.PankakeSwap.Entities.PankakeSwapPairContract;

namespace NodeBlock.Plugin.Ethereum.Nodes.BSC.PankakeSwap
{
    [NodeDefinition("OnPankakeSwapSwapNode", "On PancakeSwap Swap", NodeTypeEnum.Event, "PancakeSwap")]
    [NodeGraphDescription("Event that occur when there is a new swap on a specific pair over Pancake swap")]
    public class OnPankakeSwapSwapNode : Node, IEventEthereumNode
    {
        public OnPankakeSwapSwapNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnPankakeSwapSwapNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(object), true));
            this.InParameters.Add("contractAddress", new NodeParameter(this, "contractAddress", typeof(string), true));

            this.OutParameters.Add("sender", new NodeParameter(this, "sender", typeof(string), false));
            this.OutParameters.Add("to", new NodeParameter(this, "to", typeof(string), false));
            this.OutParameters.Add("amount0In", new NodeParameter(this, "amount0In", typeof(double), false));
            this.OutParameters.Add("amount1In", new NodeParameter(this, "amount1In", typeof(double), false));
            this.OutParameters.Add("amount0Out", new NodeParameter(this, "amount0Out", typeof(double), false));
            this.OutParameters.Add("amount1Out", new NodeParameter(this, "amount1Out", typeof(double), false));
        }

        private string contractAddress = "";
        private PancakeSwapNewSwapEvent ethLogsSubscription;

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            if (this.InParameters["contractAddress"].GetValue() != null)
                this.contractAddress = this.InParameters["contractAddress"].GetValue().ToString();

            BSCConnectorNode ethConnection = this.InParameters["connection"].GetValue() as BSCConnectorNode;

            if (ethConnection.UseManaged)
            {
                ethLogsSubscription = Plugin.EventsManagerBsc.NewEventTypePancakeSwapNewSwap(this);
                return;
            }

            var filterTransfers = Event<SwapEventDTOBase>.GetEventABI().CreateFilterInput();
            this.ethLogsSubscription = new PancakeSwapNewSwapEvent(ethConnection.SocketClient);
            ethLogsSubscription.SubscriptionDataResponse += OnEventNode;
            ethLogsSubscription.SubscribeAsync(filterTransfers).Wait();

        }

        public override void OnStop()
        {
            BSCConnectorNode ethConnection = this.InParameters["connection"].GetValue() as BSCConnectorNode;
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
            if (e.Response == null) return;
            StreamingEventArgs<Nethereum.RPC.Eth.DTOs.FilterLog> eventData = e;
            var decoded = Event<SwapEventDTOBase>.DecodeEvent(eventData.Response);
            if (decoded == null) return;
            if (!string.IsNullOrEmpty(contractAddress) && eventData.Response.Address.ToLower() != contractAddress.ToLower()) return;

            var instanciatedParameters = this.InstanciateParametersForCycle();
            instanciatedParameters["sender"].SetValue(decoded.Event.Sender);
            instanciatedParameters["to"].SetValue(decoded.Event.To);
            instanciatedParameters["amount0In"].SetValue(Web3.Convert.FromWei(decoded.Event.Amount0In));
            instanciatedParameters["amount1In"].SetValue(Web3.Convert.FromWei(decoded.Event.Amount1In));
            instanciatedParameters["amount0Out"].SetValue(Web3.Convert.FromWei(decoded.Event.Amount0Out));
            instanciatedParameters["amount1Out"].SetValue(Web3.Convert.FromWei(decoded.Event.Amount1Out));

            this.Graph.AddCycle(this, instanciatedParameters);
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
