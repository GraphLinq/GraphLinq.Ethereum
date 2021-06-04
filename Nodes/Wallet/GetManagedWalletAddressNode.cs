using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Wallet
{
    [NodeDefinition("GetManagedWalletAddressNode", "Get Managed Wallet Address", NodeTypeEnum.Function, "Ethereum Managed Wallet")]
    [NodeGraphDescription("Get the public address of the managed wallet")]
    [NodeIDEParameters(Hidden = true)]
    public class GetManagedWalletAddressNode : Node
    {
        public GetManagedWalletAddressNode(string id, BlockGraph graph)
         : base(id, graph, typeof(GetManagedWalletAddressNode).Name)
        {
            this.InParameters.Add("managedWallet", new NodeParameter(this, "managedWallet", typeof(ManagedWallet.ManagedWallet), true));
            this.OutParameters.Add("address", new NodeParameter(this, "address", typeof(string), false));

        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "address")
            {
                var managedWallet = this.InParameters["managedWallet"].GetValue() as Wallet.ManagedWallet.ManagedWallet;
                return managedWallet.ManagedWalletEntity.PublicKey;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
