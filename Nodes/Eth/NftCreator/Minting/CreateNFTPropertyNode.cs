using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.NftCreator.Minting
{
    [NodeDefinition("CreateNFTPropertyNode", "Create New Mintable NFT Token", NodeTypeEnum.Deployer, "Blockchain.Ethereum.NFT")]
    [NodeGraphDescription("Create a new NFT token from a deployed ERC-721 contract")]
    public class CreateNFTPropertyNode : Node
    {
        public CreateNFTPropertyNode(string id, BlockGraph graph)
         : base(id, graph, typeof(CreateNFTPropertyNode).Name)
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
