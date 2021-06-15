using Nethereum.Contracts;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.Uniswap.Entities.UniswapPair;

namespace NodeBlock.Plugin.Ethereum.Nodes.Uniswap
{
    [NodeDefinition("OnUniswapSwapNode", "On Uniswap Swap", NodeTypeEnum.Event, "Uniswap")]
    [NodeGraphDescription("Event that trigger on each new swap request on Uniswap from a specific token smart-contract address")]
    public class OnUniswapSwapNode : Node, IEventEthereumNode
    {
        public OnUniswapSwapNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnUniswapSwapNode).Name)
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
        private CustomUniswapSwapEvent ethLogsSubscription;

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            if (this.InParameters["contractAddress"].GetValue() != null)
                this.contractAddress = this.InParameters["contractAddress"].GetValue().ToString();

            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            if (ethConnection.UseManaged)
            {
                ethLogsSubscription = Plugin.EventsManagerEth.NewEventTypeUniswapSwap(this);
                return;
            }

            var filterTransfers = Event<SwapEventDTOBase>.GetEventABI().CreateFilterInput();
            this.ethLogsSubscription = new CustomUniswapSwapEvent(ethConnection.SocketClient);
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
            var decoded = Event<SwapEventDTOBase>.DecodeEvent(eventData.Response);
            if (decoded == null) return;
            if (!string.IsNullOrEmpty(contractAddress) && eventData.Response.Address.ToLower() != contractAddress.ToLower()) return;

            var instanciatedParameters = this.InstanciateParametersForCycle();
            instanciatedParameters["transactionHash"].SetValue(eventData.Response.TransactionHash);
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
