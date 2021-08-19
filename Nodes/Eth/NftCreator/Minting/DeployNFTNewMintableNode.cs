using System;
using System.Collections.Generic;
using System.Text;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.NftCreator.Minting
{
    [NodeDefinition("DeployNFTNewMintableNode", "Deploy New Mintable NFT Token", NodeTypeEnum.Deployer, "Blockchain.Ethereum.NFT")]
    [NodeGraphDescription("Deploy on the Ethereum network a new NFT on a specific NFT contract owned by the graph issuer")]
    public class DeployNFTNewMintableNode : Node
    {
        public DeployNFTNewMintableNode(string id, BlockGraph graph)
         : base(id, graph, typeof(DeployNFTNewMintableNode).Name)
        {

        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            return true;
        }
    }
}
