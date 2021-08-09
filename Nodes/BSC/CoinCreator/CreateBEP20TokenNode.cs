using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.BSC.CoinCreator.Models;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;


namespace NodeBlock.Plugin.Ethereum.Nodes.BSC.CoinCreator
{
    [NodeDefinition("CreateBEP20TokenNode", "Create BEP20 Token", NodeTypeEnum.Deployer, "Blockchain.BSC.ERC20")]
    [NodeGraphDescription("Create a new BEP20 token instance")]
    public class CreateBEP20TokenNode : Node
    {
        public CreateBEP20TokenNode(string id, BlockGraph graph)
          : base(id, graph, typeof(CreateBEP20TokenNode).Name)
        {
 
            this.InParameters.Add("name", new NodeParameter(this, "name", typeof(string), true));
            this.InParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), true));
            this.InParameters.Add("owner", new NodeParameter(this, "owner", typeof(string), true));
            this.InParameters.Add("totalSupply", new NodeParameter(this, "totalSupply", typeof(string), true));

            this.OutParameters.Add("bep20", new NodeParameter(this, "bep20", typeof(BEP20CreatorModel), false));
        }

        public BEP20CreatorModel BEP20Token { get; set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            this.BEP20Token = new BEP20CreatorModel();
            this.BEP20Token.Name = this.InParameters["name"].GetValue().ToString();
            this.BEP20Token.Symbol = this.InParameters["symbol"].GetValue().ToString();
            this.BEP20Token.Owner = this.InParameters["owner"].GetValue().ToString();
            this.BEP20Token.MaxSupply = BigInteger.Parse(this.InParameters["totalSupply"].GetValue().ToString());
            this.BEP20Token.InitialSupply = BigInteger.Parse(this.InParameters["totalSupply"].GetValue().ToString());

            this.OutParameters["bep20"].SetValue(this.BEP20Token);
            return true;
        }
    }
}
