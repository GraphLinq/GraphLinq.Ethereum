using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NodeBlock.Plugin.Ethereum.Nodes.Eth.NftCreator.Models;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.NftCreator.Minting
{
    [NodeDefinition("CreateNFTPropertyNode", "Create New Mintable NFT Token", NodeTypeEnum.Deployer, "Blockchain.Ethereum.NFT")]
    [NodeGraphDescription("Create a new NFT token from a deployed ERC-721 contract")]
    public class CreateNFTPropertyNode : Node
    {
        public CreateNFTPropertyNode(string id, BlockGraph graph)
         : base(id, graph, typeof(CreateNFTPropertyNode).Name)
        {
            this.InParameters.Add("contractAddress", new NodeParameter(this, "contractAddress", typeof(string), true));
            this.InParameters.Add("tokenOwner", new NodeParameter(this, "tokenOwner", typeof(string), true));
            this.InParameters.Add("name", new NodeParameter(this, "name", typeof(string), true));
            this.InParameters.Add("description", new NodeParameter(this, "description", typeof(string), true));
            this.InParameters.Add("image", new NodeParameter(this, "image", typeof(string), true));

            this.OutParameters.Add("nftToken", new NodeParameter(this, "nftToken", typeof(NFTTokenModel), false));
        }

        public NFTTokenModel nFTToken { get; set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            this.nFTToken = new NFTTokenModel();
            this.nFTToken.ContractAddress = this.InParameters["contractAddress"].GetValue().ToString();
            this.nFTToken.TokenOwner = this.InParameters["tokenOwner"].GetValue().ToString();
            this.nFTToken.Name = this.InParameters["name"].GetValue().ToString();
            this.nFTToken.Description = this.InParameters["description"].GetValue().ToString();
            this.nFTToken.ImageUrl = this.InParameters["image"].GetValue().ToString();

            this.OutParameters["nftToken"].SetValue(this.nFTToken);
            return true;
        }
    }
}
