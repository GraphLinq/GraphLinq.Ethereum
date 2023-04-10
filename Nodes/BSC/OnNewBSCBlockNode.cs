using System;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;

namespace NodeBlock.Plugin.Ethereum.Nodes.BSC
{
    [NodeDefinition("OnNewBscBlockNode", "On Binance Smart Chain Block", NodeTypeEnum.Event, "Blockchain.BSC")]
    [NodeGraphDescription("Event that occurs everytime a new bsc block is minted")]
    public class OnNewBSCBlockNode : Node, IEventEthereumNode
    {
        private EthNewBlockHeadersObservableSubscription blockHeadersSubscription;

        public OnNewBSCBlockNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnNewBSCBlockNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(string), true));

            this.OutParameters.Add("block", new NodeParameter(this, "block", typeof(Nethereum.RPC.Eth.DTOs.Block), true));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            BSCConnectorNode ethConnection = this.InParameters["connection"].GetValue() as BSCConnectorNode;
            if (ethConnection.UseManaged)
            {
                blockHeadersSubscription = Plugin.EventsManagerBsc.NewEventTypePendingBlocks(this);
            }
            else
            {
                this.blockHeadersSubscription = new EthNewBlockHeadersObservableSubscription(ethConnection.SocketClient);
                blockHeadersSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(async Block =>
                {
                    var instanciatedParameters = this.InstanciatedParametersForCycle();
                    instanciatedParameters["block"].SetValue(Block);

                    this.Graph.AddCycle(this, instanciatedParameters);
                });
                blockHeadersSubscription.SubscribeAsync();
            }

        }

        public override void OnStop()
        {
            BSCConnectorNode ethConnection = this.InParameters["connection"].GetValue() as BSCConnectorNode;
            if (ethConnection.UseManaged)
            {
                string eventType = blockHeadersSubscription.GetType().ToString();
                Plugin.EventsManagerBsc.RemoveEventNode(eventType, this);
                return;
            }
            this.blockHeadersSubscription.UnsubscribeAsync().Wait();
        }

        public override void BeginCycle()
        {
            this.Next();
        }

        public void OnEventNode(object sender, dynamic e)
        {
            var instanciatedParameters = this.InstanciatedParametersForCycle();
            instanciatedParameters["block"].SetValue(e);

            this.Graph.AddCycle(this, instanciatedParameters);
        }
    }
}