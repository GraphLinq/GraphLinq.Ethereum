using Nethereum.Contracts;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.BSC.PankakeSwap.Entities.PancakeSwapFactory;
using static NodeBlock.Plugin.Ethereum.Nodes.BSC.PankakeSwap.Entities.PankakeSwapPairContract;

namespace NodeBlock.Plugin.Ethereum.Nodes.BSC.PankakeSwap
{
    [NodeDefinition("OnPankakeSwapNewPairNode", "On PancakeSwap New Pair", NodeTypeEnum.Event, "PancakeSwap")]
    [NodeGraphDescription("Event that occur when there is a new pair created over Pancake swap")]
    public class OnPankakeSwapNewPairNode : Node, IEventEthereumNode
    {
        public OnPankakeSwapNewPairNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnPankakeSwapNewPairNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(object), true));

            this.OutParameters.Add("token0", new NodeParameter(this, "token0", typeof(string), false));
            this.OutParameters.Add("token1", new NodeParameter(this, "token1", typeof(string), false));
            this.OutParameters.Add("pair", new NodeParameter(this, "pair", typeof(string), false));
        }

        private string contractAddress = "";
        private PancakeSwapNewPairEvent ethLogsSubscription;

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;


        public override void SetupEvent()
        {
            this.contractAddress = "0xbcfccbde45ce874adcb698cc183debcf17952812";

            BSCConnectorNode ethConnection = this.InParameters["connection"].GetValue() as BSCConnectorNode;

            if (ethConnection.UseManaged)
            {
                ethLogsSubscription = Plugin.EventsManagerBsc.NewEventTypePancakeSwapNewPair(this);
                return;
            }

            var filterTransfers = Event<PairCreatedEventDTOBase>.GetEventABI().CreateFilterInput(this.contractAddress);
            this.ethLogsSubscription = new PancakeSwapNewPairEvent(ethConnection.SocketClient);
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
            try
            {
                StreamingEventArgs<Nethereum.RPC.Eth.DTOs.FilterLog> eventData = e;
                if (!string.IsNullOrEmpty(contractAddress) && eventData.Response.Address.ToLower() != contractAddress.ToLower()) return;
                var decoded = Event<PairCreatedEventDTOBase>.DecodeEvent(eventData.Response);
                if (decoded == null) return;

                var instanciatedParameters = this.InstanciatedParametersForCycle();
                instanciatedParameters["token0"].SetValue(decoded.Event.Token0);
                instanciatedParameters["token1"].SetValue(decoded.Event.Token1);
                instanciatedParameters["pair"].SetValue(decoded.Event.Pair);

                this.Graph.AddCycle(this, instanciatedParameters);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
