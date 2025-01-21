using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.GraphLinq.CoinCreator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using NodeBlock.Plugin.Ethereum.Nodes.Wallet.ManagedWallet;
using Nethereum.Web3.Accounts.Managed;
using NodeBlock.Plugin.Ethereum.Nodes.GraphLinq.Contracts;

namespace NodeBlock.Plugin.Ethereum.Nodes.GraphLinq.CoinCreator
{
    [NodeDefinition("DeployERC20GlqNode", "Deploy ERC20 GLQ Token", NodeTypeEnum.Deployer, "Blockchain.GraphLinq.ERC20")]
    [NodeGraphDescription("Deploy the ERC20 GLQ token")]
    public class DeployERC20GlqNode : Node
    {
        public DeployERC20GlqNode(string id, BlockGraph graph)
          : base(id, graph, typeof(DeployERC20GlqNode).Name)
        {
            this.InParameters.Add("wallet", new NodeParameter(this, "wallet", typeof(ManagedWallet), true));
            this.InParameters.Add("erc20", new NodeParameter(this, "erc20", typeof(ERC20GlqCreatorModel), true));

            this.OutParameters.Add("contractAddress", new NodeParameter(this, "contractAddress", typeof(string), false));
        }

        public Models.ERC20GlqCreatorModel ERC20Token { get; set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            ManagedWallet wallet = (this.InParameters["wallet"].GetValue() as ManagedWallet);
            var erc20 = (this.InParameters["erc20"].GetValue() as ERC20GlqCreatorModel);

            var account = wallet.GetAccount();
            var web3 = new Nethereum.Web3.Web3(account, Environment.GetEnvironmentVariable("glq_chain_rpc_url"));

            var deploymentMessage = new StandardGlqTokenContract
            {
                Deployer = erc20.Owner,
                Name = erc20.Name,
                Symbol = erc20.Symbol,
                Decimals = 18,
                Supply = erc20.MaxSupply
            };

            var deploymentHandler = web3.Eth.GetContractDeploymentHandler<StandardGlqTokenContract>();
            try
            {
                this.Graph.AppendLog("info", string.Format("Deploying token {0} contract with wallet {1} ({2}): owner -> {3} ", erc20.Name, account.Address, wallet.ManagedWalletEntity.Name, erc20.Owner));
                var transactionReceipt1 = deploymentHandler.SendRequestAndWaitForReceiptAsync(deploymentMessage).Result;
                var address = transactionReceipt1.ContractAddress;

                this.OutParameters["contractAddress"].SetValue(address);
                return true;
            }
            catch (Exception error)
            {

                this.Graph.AppendLog("error", error.ToString());
                return false;
            }
        }
    }
}