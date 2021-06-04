using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Wallet
{
    [NodeDefinition("SetGasForWalletNode", "Set Gas For Managed Wallet", NodeTypeEnum.Function, "Ethereum Managed Wallet")]
    [NodeGraphDescription("Set gas cost for a managed wallet")]
    [NodeIDEParameters(Hidden = true)]
    public class SetGasForWalletNode : Node
    {
        public SetGasForWalletNode(string id, BlockGraph graph)
            : base(id, graph, typeof(SetGasForWalletNode).Name)
        {
            this.InParameters.Add("gwei", new NodeParameter(this, "gwei", typeof(int), true));
            this.InParameters.Add("managedWallet", new NodeParameter(this, "managedWallet", typeof(Wallet.ManagedWallet.ManagedWallet), true));
        }
        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var gwei = int.Parse(this.InParameters["gwei"].GetValue().ToString());
            var managedWallet = this.InParameters["managedWallet"].GetValue() as Wallet.ManagedWallet.ManagedWallet;
            managedWallet.Gwei = gwei;
            return true;
        }
    }
}
