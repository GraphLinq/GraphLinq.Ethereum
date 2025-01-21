using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.GraphLinq.CoinCreator.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;


namespace NodeBlock.Plugin.Ethereum.Nodes.GraphLinq.CoinCreator
{
    [NodeDefinition("CreateERC20GlqTokenNode", "Create ERC20 GLQ Token", NodeTypeEnum.Deployer, "Blockchain.GraphLinq.ERC20")]
    [NodeGraphDescription("Create a new ERC20 GLQ token instance")]
    public class CreateERC20GlqTokenNode : Node
    {
        public CreateERC20GlqTokenNode(string id, BlockGraph graph)
          : base(id, graph, typeof(CreateERC20GlqTokenNode).Name)
        {

            this.InParameters.Add("name", new NodeParameter(this, "name", typeof(string), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));
            this.InParameters.Add("owner", new NodeParameter(this, "owner", typeof(string), true));
            this.InParameters.Add("totalSupply", new NodeParameter(this, "totalSupply", typeof(string), true));

            this.OutParameters.Add("erc20", new NodeParameter(this, "erc20", typeof(ERC20GlqCreatorModel), false));
        }

        public ERC20GlqCreatorModel ERC20Token { get; set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            this.ERC20Token = new ERC20GlqCreatorModel();
            this.ERC20Token.Name = this.InParameters["name"].GetValue().ToString();
            this.ERC20Token.Symbol = this.InParameters["symbol"].GetValue().ToString();
            this.ERC20Token.Owner = this.InParameters["owner"].GetValue().ToString();
            this.ERC20Token.MaxSupply = BigInteger.Parse(this.InParameters["totalSupply"].GetValue().ToString());
            this.ERC20Token.InitialSupply = BigInteger.Parse(this.InParameters["totalSupply"].GetValue().ToString());

            this.OutParameters["erc20"].SetValue(this.ERC20Token);
            return true;
        }
    }
}