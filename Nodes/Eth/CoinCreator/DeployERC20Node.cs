using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.Eth.CoinCreator.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.CoinCreator
{
    [NodeDefinition("DeployERC20Node", "Deploy ERC20 Token", NodeTypeEnum.Deployer, "Blockchain.Ethereum.ERC20")]
    [NodeGraphDescription("Deploy the ERC20 token")]
    public class DeployERC20Node : Node
    {
        public DeployERC20Node(string id, BlockGraph graph)
          : base(id, graph, typeof(DeployERC20Node).Name)
        {
            this.InParameters.Add("erc20", new NodeParameter(this, "erc20", typeof(ERC20CreatorModel), true));
        }

        public Models.ERC20CreatorModel ERC20Token { get; set; }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var erc20 = (this.InParameters["erc20"].GetValue() as ERC20CreatorModel);
            return true;
        }
    }
}
