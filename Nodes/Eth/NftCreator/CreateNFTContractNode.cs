using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using NodeBlock.Plugin.Ethereum.Nodes.Eth.NftCreator.Models;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.NftCreator
{
    [NodeDefinition("CreateNFTContractNode", "Create NFT Contract", NodeTypeEnum.Deployer, "Blockchain.Ethereum.NFT")]
    [NodeGraphDescription("Create a NFT contract ready for deployment for the Ethereum Network")]

    public class CreateNFTContractNode : Node
    {
        public CreateNFTContractNode(string id, BlockGraph graph)
            : base(id, graph, typeof(CreateNFTContractNode).Name)
        {
            this.InParameters.Add("ownerAddress", new NodeParameter(this, "ownerAddress", typeof(string), true));
            this.InParameters.Add("name", new NodeParameter(this, "name", typeof(string), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));

            this.OutParameters.Add("nftModel", new NodeParameter(this, "nftModel", typeof(NFTCreatorModel), false));
        }

        public NFTCreatorModel NFTModel { get; set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            this.NFTModel = new NFTCreatorModel();
            this.NFTModel.Owner = this.InParameters["ownerAddress"].GetValue().ToString();
            this.NFTModel.Name = this.InParameters["name"].GetValue().ToString();
            this.NFTModel.Symbol = this.InParameters["symbol"].GetValue().ToString();

            this.OutParameters["nftModel"].SetValue(this.NFTModel);
            return true;
        }
    }


}
