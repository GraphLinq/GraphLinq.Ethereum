using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.Eth.CoinCreator.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.CoinCreator
{
    [NodeDefinition("CreateERC20TokenNode", "Create ERC20 Token", NodeTypeEnum.Deployer, "Blockchain.Ethereum.ERC20")]
    [NodeGraphDescription("Create a new ERC20 token instance")]
    public class CreateERC20TokenNode : Node
    {
        public CreateERC20TokenNode(string id, BlockGraph graph)
          : base(id, graph, typeof(CreateERC20TokenNode).Name)
        {
            this.InParameters.Add("name", new NodeParameter(this, "name", typeof(string), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));
            this.InParameters.Add("owner", new NodeParameter(this, "owner", typeof(string), true));
            this.InParameters.Add("maxSupply", new NodeParameter(this, "maxSupply", typeof(string), true));
            this.InParameters.Add("initialSupply", new NodeParameter(this, "initialSupply", typeof(string), true));

            this.OutParameters.Add("erc20", new NodeParameter(this, "erc20", typeof(ERC20CreatorModel), false));
        }

        public ERC20CreatorModel ERC20Token { get; set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            this.ERC20Token = new ERC20CreatorModel();
            this.ERC20Token.Name = this.InParameters["name"].GetValue().ToString();
            this.ERC20Token.Symbol = this.InParameters["symbol"].GetValue().ToString();
            this.ERC20Token.Owner = this.InParameters["owner"].GetValue().ToString();
            this.ERC20Token.MaxSupply = BigInteger.Parse(this.InParameters["maxSupply"].GetValue().ToString());
            this.ERC20Token.InitialSupply = BigInteger.Parse(this.InParameters["initialSupply"].GetValue().ToString());

            this.OutParameters["erc20"].SetValue(this.ERC20Token);
            return true;
        }
    }
}
