using Nethereum.Contracts;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
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
    [NodeDefinition("OnQuickswapSwapNode", "On Quickswap Swap", NodeTypeEnum.Event, "Quickswap")]
    [NodeGraphDescription("Event that trigger on each new swap request on Quickswap from a specific token smart-contract address")]
    [NodeIDEParameters(Hidden = false)]
    public class OnQuickswapSwapNode : Node
    {
        public OnQuickswapSwapNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnQuickswapSwapNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(string), true));
            this.InParameters.Add("contractAddress", new NodeParameter(this, "contractAddress", typeof(string), true));

            this.OutParameters.Add("transactionHash", new NodeParameter(this, "transactionHash", typeof(string), false));
            this.OutParameters.Add("sender", new NodeParameter(this, "sender", typeof(string), false));
            this.OutParameters.Add("to", new NodeParameter(this, "to", typeof(string), false));
            this.OutParameters.Add("amount0In", new NodeParameter(this, "amount0In", typeof(double), false));
            this.OutParameters.Add("amount1In", new NodeParameter(this, "amount1In", typeof(double), false));
            this.OutParameters.Add("amount0Out", new NodeParameter(this, "amount0Out", typeof(double), false));
            this.OutParameters.Add("amount1Out", new NodeParameter(this, "amount1Out", typeof(double), false));
        }

        private string contractAddress = "";
        private EthLogsObservableSubscription ethLogsSubscription;

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            if (this.InParameters["contractAddress"].GetValue() != null)
                this.contractAddress = this.InParameters["contractAddress"].GetValue().ToString();

            PolygonConnectorNode ethConnection = this.InParameters["connection"].GetValue() as PolygonConnectorNode;
            var filterTransfers = ethConnection.Web3Client.Eth.GetEvent<SwapEventDTOBase>(this.contractAddress).CreateFilterInput();
            this.ethLogsSubscription = new EthLogsObservableSubscription(ethConnection.SocketClient);
            this.ethLogsSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
            {
                if (!string.IsNullOrEmpty(contractAddress) && log.Address.ToLower() != contractAddress.ToLower()) return;
                var decoded = Event<SwapEventDTOBase>.DecodeEvent(log);
                if (decoded == null) return;
                OnEvent(decoded.Event, log.TransactionHash);
            });
            this.ethLogsSubscription.SubscribeAsync(filterTransfers);
        }

        public override void OnStop()
        {
            PolygonConnectorNode ethConnection = this.InParameters["connection"].GetValue() as PolygonConnectorNode;
            this.ethLogsSubscription.UnsubscribeAsync().Wait();
        }
    
        private void OnEvent(SwapEventDTOBase evt, string txHash)
        {
            var instanciatedParameters = this.InstanciateParametersForCycle();
            instanciatedParameters["transactionHash"].SetValue(txHash);
            instanciatedParameters["sender"].SetValue(evt.Sender);
            instanciatedParameters["to"].SetValue(evt.To);
            instanciatedParameters["amount0In"].SetValue(Web3.Convert.FromWei(evt.Amount0In));
            instanciatedParameters["amount1In"].SetValue(Web3.Convert.FromWei(evt.Amount1In));
            instanciatedParameters["amount0Out"].SetValue(Web3.Convert.FromWei(evt.Amount0Out));
            instanciatedParameters["amount1Out"].SetValue(Web3.Convert.FromWei(evt.Amount1Out));

            this.Graph.AddCycle(this, instanciatedParameters);
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
