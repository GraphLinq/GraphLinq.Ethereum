using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Ethereum.Nodes.BSC.Wallet
{
    [NodeDefinition("OnNewBscWalletTransaction", "On Binance Smart Chain Wallet Transaction", NodeTypeEnum.Event, "Blockchain.BSC")]
    [NodeGraphDescription("Event that watch a specific wallet addr and return the details of any new transaction")]
    public class OnBSCWalletTransaction : Node, IEventEthereumNode
    {
        private EthNewBlockHeadersObservableSubscription blockHeadersSubscription;

        public OnBSCWalletTransaction(string id, BlockGraph graph)
            : base(id, graph, typeof(OnBSCWalletTransaction).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(EthConnection), true));
            this.InParameters.Add("walletAddress", new NodeParameter(this, "walletAddress", typeof(string), true));

            this.OutParameters.Add("transaction", new NodeParameter(this, "transaction", typeof(Transaction), true));
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
                    onBlock(Block);
                });
                blockHeadersSubscription.SubscribeAsync();
            }
        }

        private async void onBlock(Block Block)
        {
            BSCConnectorNode ethConnection = this.InParameters["connection"].GetValue() as BSCConnectorNode;
            var blockInformations = await ethConnection.Web3Client.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(Block.Number);
            foreach (var tx in blockInformations.Transactions)
            {
                var instanciatedParameters = this.InstanciatedParametersForCycle();

                if (tx.From != null && this.InParameters["walletAddress"].GetValue().ToString().ToLower() == tx.From.ToLower())
                {
                    instanciatedParameters["transaction"].SetValue(tx);
                    this.Graph.AddCycle(this, instanciatedParameters);
                }
                else if (tx.To != null && this.InParameters["walletAddress"].GetValue().ToString().ToLower() == tx.To.ToLower())
                {
                    instanciatedParameters["transaction"].SetValue(tx);
                    this.Graph.AddCycle(this, instanciatedParameters);
                }
            }
        }

        public override void OnStop()
        {
            BSCConnectorNode ethConnection = this.InParameters["connection"].GetValue() as BSCConnectorNode;
            if (ethConnection.UseManaged)
            {
                if (this.blockHeadersSubscription != null) Plugin.EventsManagerBsc.RemoveEventNode(blockHeadersSubscription.GetType().ToString(), this);
            }
            else
            {
                if (this.blockHeadersSubscription != null)
                    this.blockHeadersSubscription.UnsubscribeAsync().Wait();
            }
        }

        public override void BeginCycle()
        {
            this.Next();
        }

        public void OnEventNode(object sender, dynamic e)
        {
            onBlock(e);
        }
    }
}
