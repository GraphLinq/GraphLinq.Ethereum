using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.Subscriptions;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using NodeBlock.Plugin.Ethereum;

namespace NodeBlock.Plugin.Ethereum.Nodes.BSC
{
    [NodeDefinition("OnNewBscTxNode", "On Binance Smart Chain Transaction", NodeTypeEnum.Event, "Blockchain.BSC")]
    [NodeGraphDescription("Event that occurs everytime a new bsc transaction appears in the last network block")]
    public class OnNewBscTxNode : Node, IEventEthereumNode
    {
        private EthNewPendingTransactionSubscription ethNewPendingTransactionSubscription;

        public OnNewBscTxNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnNewBscTxNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(string), true));

            this.OutParameters.Add("transactionHash", new NodeParameter(this, "transactionHash", typeof(string), true));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            BSCConnectorNode ethConnection = this.InParameters["connection"].GetValue() as BSCConnectorNode;
            if (ethConnection.UseManaged)
            {
                ethNewPendingTransactionSubscription = Plugin.EventsManagerBsc.NewEventTypePendingTxs(this);
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
            BSCConnectorNode ethConnection = this.InParameters["connection"].GetValue() as BSCConnectorNode;
            if (ethConnection.UseManaged)
            {
                string eventType = ethNewPendingTransactionSubscription.GetType().ToString();
                Plugin.EventsManagerBsc.RemoveEventNode(eventType, this);
                return;
            }
            this.ethNewPendingTransactionSubscription.UnsubscribeAsync().Wait();
        }

        public void OnEventNode(object sender, dynamic e)
        {
            StreamingEventArgs<string> eventData = e;

            if (eventData.Response == null) return;
            var instanciatedParameters = this.InstanciatedParametersForCycle();
            instanciatedParameters["transactionHash"].SetValue(eventData.Response);

            this.Graph.AddCycle(this, instanciatedParameters);
        }
        public override void BeginCycle()
        {
            this.Next();
        }

    }
}
