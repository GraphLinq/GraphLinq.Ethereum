using Nethereum.Contracts;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.BSC;
using System;
using System.Collections.Generic;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.Unicrypt.Entities.UniswapV2Locker;

namespace NodeBlock.Plugin.Ethereum.Nodes.Unicrypt
{
    [NodeDefinition("OnUnicryptDepositNode", "On Unicrypt Deposit", NodeTypeEnum.Event, "Unicrypt")]
    [NodeGraphDescription("Event that trigger when someone deposit lp in Unicrypt")]
    public class OnUnicryptDepositNode : Node
    {
        public OnUnicryptDepositNode(string id, BlockGraph graph)
            : base(id, graph, typeof(OnUnicryptDepositNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("ethereumConnection", new NodeParameter(this, "ethereumConnection", typeof(object), true));
            this.InParameters.Add("bscConnection", new NodeParameter(this, "bscConnection", typeof(object), true));

            this.OutParameters.Add("lpToken", new NodeParameter(this, "lpToken", typeof(string), false));
            this.OutParameters.Add("user", new NodeParameter(this, "user", typeof(string), false));
            this.OutParameters.Add("amount", new NodeParameter(this, "amount", typeof(long), false));
            this.OutParameters.Add("lockDate", new NodeParameter(this, "lockDate", typeof(long), false));
            this.OutParameters.Add("unlockDate", new NodeParameter(this, "unlockDate", typeof(long), false));
        }

        private string contractAddress = "0x663A5C229c09b049E36dCc11a9B0d4a8Eb9db214";
        private EthLogsObservableSubscription ethLogsSubscription;
        private EthLogsSubscription bscLogsSubscription;

        public override bool CanBeExecuted => false;
        public override bool CanExecute => true;

        public override void SetupEvent()
        {
            if (this.InParameters["ethereumConnection"].GetValue() != null)
            {
                EthConnection ethConnection = this.InParameters["ethereumConnection"].GetValue() as EthConnection;

                var filterTransfers = ethConnection.Web3Client.Eth.GetEvent<OnDepositEventDTOBase>(this.contractAddress).CreateFilterInput();
                this.ethLogsSubscription = new EthLogsObservableSubscription(ethConnection.SocketClient);
                this.ethLogsSubscription.GetSubscriptionDataResponsesAsObservable().Subscribe(log =>
                {
                    try
                    { 
                        if (!string.IsNullOrEmpty(contractAddress) && log.Address.ToLower() != contractAddress.ToLower()) return;
                        var decoded = Event<OnDepositEventDTOBase>.DecodeEvent(log);
                        if (decoded == null) return;

                        var instanciatedParameters = this.InstanciateParametersForCycle();
                        instanciatedParameters["lpToken"].SetValue(decoded.Event.LpToken);
                        instanciatedParameters["user"].SetValue(decoded.Event.User);
                        instanciatedParameters["amount"].SetValue(Web3.Convert.FromWei(decoded.Event.Amount));
                        instanciatedParameters["lockDate"].SetValue(decoded.Event.LockDate);
                        instanciatedParameters["lockDate"].SetValue(decoded.Event.UnlockDate);

                        this.Graph.AddCycle(this, instanciatedParameters);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                });
                ethLogsSubscription.SubscribeAsync(filterTransfers);
            }

            if (this.InParameters["bscConnection"].GetValue() != null)
            {
                //BSCConnectorNode bscConnection = this.InParameters["bscConnection"].GetValue() as BSCConnectorNode;

                //var filterTransfers = Event<OnDepositEventDTOBase>.GetEventABI().CreateFilterInput(this.contractAddress);
                //this.ethLogsSubscription = new EthLogsSubscription(bscConnection.SocketClient);
                //ethLogsSubscription.SubscriptionDataResponse += OnEventNode;
                //ethLogsSubscription.SubscribeAsync(filterTransfers);
            }
        }

        public override void OnStop()
        {
            if (this.ethLogsSubscription != null) this.ethLogsSubscription.UnsubscribeAsync();
        }

        public override void BeginCycle()
        {
            this.Next();
        }
    }
}
