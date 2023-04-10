using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.BSC.CoinCreator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using NodeBlock.Plugin.Ethereum.Nodes.Wallet.ManagedWallet;
using Nethereum.Web3.Accounts.Managed;
using NodeBlock.Plugin.Ethereum.Nodes.Eth.Contracts;

namespace NodeBlock.Plugin.Ethereum.Nodes.BSC.CoinCreator
{
    [NodeDefinition("DeployBEP20Node", "Deploy BEP20 Token", NodeTypeEnum.Deployer, "Blockchain.BSC.BEP20")]
    [NodeGraphDescription("Deploy the BEP20 token")]
    public class DeployBEP20Node : Node
    {
        public DeployBEP20Node(string id, BlockGraph graph)
          : base(id, graph, typeof(DeployBEP20Node).Name)
        {
            this.InParameters.Add("wallet", new NodeParameter(this, "wallet", typeof(ManagedWallet), true));
            this.InParameters.Add("bep20", new NodeParameter(this, "bep20", typeof(BEP20CreatorModel), true));

            this.OutParameters.Add("contractAddress", new NodeParameter(this, "contractAddress", typeof(string), false));
        }

        public BEP20CreatorModel BEP20Token { get; set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            ManagedWallet wallet = (this.InParameters["wallet"].GetValue() as ManagedWallet);
            var erc20 = (this.InParameters["bep20"].GetValue() as BEP20CreatorModel);

            var account = wallet.GetAccount();
            var web3 = new Nethereum.Web3.Web3(account, Environment.GetEnvironmentVariable("bsc_api_http_url"));

            var deploymentMessage = new StandardTokenContract
            {
                Deployer = erc20.Owner,
                Name = erc20.Name,
                Symbol = erc20.Symbol,
                Decimals = 18,
                Supply = erc20.MaxSupply
            };

            var deploymentHandler = web3.Eth.GetContractDeploymentHandler<StandardTokenContract>();
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
