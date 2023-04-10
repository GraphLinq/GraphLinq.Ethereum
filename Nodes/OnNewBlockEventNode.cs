using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes
{
    [NodeDefinition("OnNewBlockEventNode", "On Ethereum Block", NodeTypeEnum.Event, "Blockchain.Ethereum")]
    [NodeGraphDescription("Event that occurs every time a new Ethereum block is minted")]
    public class OnNewBlockEventNode : Node, IEventEthereumNode
    {
        private EthNewBlockHeadersObservableSubscription blockHeadersSubscription;

        public OnNewBlockEventNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnNewBlockEventNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(string), true));

            this.OutParameters.Add("block", new NodeParameter(this, "block", typeof(Nethereum.RPC.Eth.DTOs.Block), true));

        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            if (ethConnection.UseManaged)
            {
                blockHeadersSubscription = Plugin.EventsManagerEth.NewEventTypePendingBlocks(this);
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
            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            if (ethConnection.UseManaged)
            {
                string eventType = blockHeadersSubscription.GetType().ToString();
                Plugin.EventsManagerEth.RemoveEventNode(eventType, this);
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
