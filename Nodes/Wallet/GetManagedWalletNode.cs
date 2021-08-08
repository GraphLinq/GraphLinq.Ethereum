using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Wallet
{
    [NodeDefinition("GetManagedWallet", "Get Personal Managed Wallet", NodeTypeEnum.Function, "Ethereum Managed Wallet")]
    [NodeGraphDescription("Get an Ethereum Managed Wallet for on-chain operation from your created accounts on the Dapp")]
    [NodeSpecialActionAttribute("How managed wallet work ?", "open_url", "#")]
    [NodeIDEParameters(Hidden = true)]
    public class GetManagedWallet : Node
    {
        public GetManagedWallet(string id, BlockGraph graph)
          : base(id, graph, typeof(CreateManagedWalletNode).Name)
        {
            this.InParameters.Add("walletName", new NodeParameter(this, "walletName", typeof(string), true));

            this.OutParameters.Add("managedWallet", new NodeParameter(this, "managedWallet", typeof(ManagedWallet.ManagedWallet), false));
            this.OutParameters.Add("address", new NodeParameter(this, "address", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var name = this.InParameters["walletName"].GetValue().ToString();   
            var managedWallet = ManagedWallet.ManagedWallet.GetOrCreateManagedWallet(this.Graph.currentContext.walletIdentifier, name);

            this.OutParameters["managedWallet"].SetValue(managedWallet);
            this.OutParameters["address"].SetValue(managedWallet.ManagedWalletEntity.PublicKey);

            return true;
        }
    }
}
