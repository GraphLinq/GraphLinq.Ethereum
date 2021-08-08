using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.Eth.CoinCreator.Models;
using System;
using System.Collections.Generic;
using System.Text;
using NodeBlock.Plugin.Ethereum.Nodes.Wallet.ManagedWallet;
using Nethereum.Web3.Accounts.Managed;
using NodeBlock.Plugin.Ethereum.Nodes.Eth.Contracts;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.CoinCreator
{
    [NodeDefinition("DeployERC20Node", "Deploy ERC20 Token", NodeTypeEnum.Deployer, "Blockchain.Ethereum.ERC20")]
    [NodeGraphDescription("Deploy the ERC20 token")]
    public class DeployERC20Node : Node
    {
        public DeployERC20Node(string id, BlockGraph graph)
          : base(id, graph, typeof(DeployERC20Node).Name)
        {
            this.InParameters.Add("wallet", new NodeParameter(this, "wallet", typeof(ManagedWallet), true));
            this.InParameters.Add("erc20", new NodeParameter(this, "erc20", typeof(ERC20CreatorModel), true));
        }

        public Models.ERC20CreatorModel ERC20Token { get; set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            ManagedWallet wallet = (this.InParameters["wallet"].GetValue() as ManagedWallet);
            var erc20 = (this.InParameters["erc20"].GetValue() as ERC20CreatorModel);

            var account = wallet.GetAccount();
            var web3 = new Nethereum.Web3.Web3(account, Environment.GetEnvironmentVariable("eth_api_http_url"));

            var deploymentMessage = new StandardTokenContract
            {
                Deployer = erc20.Owner,
                Name = erc20.Name,
                Symbol = erc20.Symbol,
                InitialSupply = erc20.InitialSupply,
                TotalSupply = erc20.MaxSupply
            };

            var deploymentHandler = web3.Eth.GetContractDeploymentHandler<StandardTokenContract>();
            var transactionReceipt1 = deploymentHandler.SendRequestAndWaitForReceiptAsync(deploymentMessage).Result;
            var address = transactionReceipt1.ContractAddress;

            this.OutParameters["contractAddress"].SetValue(address);
            return true;
        }
    }
}
