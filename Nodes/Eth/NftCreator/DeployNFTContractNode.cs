using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NodeBlock.Plugin.Ethereum.Nodes.Eth.NftCreator.Models;
using NodeBlock.Plugin.Ethereum.Nodes.Eth.Contracts;
using NodeBlock.Plugin.Ethereum.Nodes.Wallet.ManagedWallet;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.NftCreator
{
    [NodeDefinition("DeployNFTContractNode", "Deploy NFT Contract", NodeTypeEnum.Deployer, "Blockchain.Ethereum.NFT")]
    [NodeGraphDescription("Deploy a NFT contract (ERC-721) on the Ethereum network")]
    public class DeployNFTContractNode : Node
    {
        public DeployNFTContractNode(string id, BlockGraph graph)
            : base(id, graph, typeof(DeployNFTContractNode).Name)
        {
            this.InParameters.Add("wallet", new NodeParameter(this, "wallet", typeof(ManagedWallet), true));
            this.InParameters.Add("nftModel", new NodeParameter(this, "nftModel", typeof(NFTCreatorModel), true));

            this.OutParameters.Add("contractAddress", new NodeParameter(this, "contractAddress", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            ManagedWallet wallet = (this.InParameters["wallet"].GetValue() as ManagedWallet);
            var nftModel = (this.InParameters["nftModel"].GetValue() as NFTCreatorModel);

            var account = wallet.GetAccount();
            var web3 = new Nethereum.Web3.Web3(account, Environment.GetEnvironmentVariable("eth_api_http_url"));

            var deploymentMessage = new StandardNftContract()
            {
                Owner = nftModel.Owner,
                Name = nftModel.Name,
                Symbol = nftModel.Symbol
            };

            var deploymentHandler = web3.Eth.GetContractDeploymentHandler<StandardNftContract>();
            try
            {
                this.Graph.AppendLog("info", string.Format("Deploying NFT ERC-721 {0} contract with wallet {1} ({2}): owner -> {3} ", nftModel.Name, account.Address, wallet.ManagedWalletEntity.Name, nftModel.Owner));
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
