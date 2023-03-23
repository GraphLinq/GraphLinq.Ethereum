using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.Subscriptions;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using NodeBlock.Plugin.Ethereum;

namespace NodeBlock.Plugin.Ethereum.Nodes
{
    [NodeDefinition("OnNewTransactionEventNode", "On Ethereum Transaction", NodeTypeEnum.Event, "Blockchain.Ethereum")]
    [NodeGraphDescription("Event that occurs every time a new Ethereum transaction appears in the last network block")]
    public class OnNewTransactionEventNode : Node, IEventEthereumNode
    {
        private EthNewPendingTransactionSubscription ethNewPendingTransactionSubscription;

        public OnNewTransactionEventNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnNewTransactionEventNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(string), true));

            this.OutParameters.Add("transactionHash", new NodeParameter(this, "transactionHash", typeof(string), true));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            if (ethConnection.UseManaged)
            {
                ethNewPendingTransactionSubscription = Plugin.EventsManagerEth.NewEventTypePendingTxs(this);
            }
            else
            {
                this.ethNewPendingTransactionSubscription = new EthNewPendingTransactionSubscription(ethConnection.SocketClient);
                ethNewPendingTransactionSubscription.SubscriptionDataResponse += OnEventNode;
                ethNewPendingTransactionSubscription.SubscribeAsync().Wait();
            }
        }

        public override void OnStop()
        {
            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            if (ethConnection.UseManaged)
            {
                string eventType = ethNewPendingTransactionSubscription.GetType().ToString();
                Plugin.EventsManagerEth.RemoveEventNode(eventType, this);
                return;
            }
            this.ethNewPendingTransactionSubscription.UnsubscribeAsync().Wait();
        }

        public void OnEventNode(object sender, dynamic e)
        {
            StreamingEventArgs<string> eventData = e;

            if (eventData.Response == null) return;
            var instanciatedParameters = this.InstanciateParametersForCycle();
            instanciatedParameters["transactionHash"].SetValue(eventData.Response);

            this.Graph.AddCycle(this, instanciatedParameters);
        }
        public override void BeginCycle()
        {
            this.Next();
        }

    }
}
