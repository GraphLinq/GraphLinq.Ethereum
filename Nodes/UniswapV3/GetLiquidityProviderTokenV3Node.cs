using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.UniswapV3.Entities.UniswapV3ERC20;

namespace NodeBlock.Plugin.Ethereum.Nodes.UniswapV3
{
    [NodeDefinition("GetLiquidityProviderTokenV3Node", "Get Uniswap v3 LP Token", NodeTypeEnum.Function, "Uniswap v3")]
    [NodeGraphDescription("Return all information about a v3 liquidity provider token")]
    public class GetLiquidityProviderTokenV3Node : Node
    {
        public GetLiquidityProviderTokenV3Node(string id, BlockGraph graph)
            : base(id, graph, typeof(GetLiquidityProviderTokenV3Node).Name)
        {
            this.InParameters.Add("ethConnection", new NodeParameter(this, "ethConnection", typeof(object), true));
            this.InParameters.Add("address", new NodeParameter(this, "address", typeof(string), true));

            this.OutParameters.Add("name", new NodeParameter(this, "name", typeof(string), false));
            this.OutParameters.Add("token0", new NodeParameter(this, "token0", typeof(string), false));
            this.OutParameters.Add("token1", new NodeParameter(this, "token1", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            EthConnection ethConnection = this.InParameters["ethConnection"].GetValue() as EthConnection;
            var contractHandler = ethConnection.Web3Client.Eth.GetContractHandler(this.InParameters["address"].GetValue().ToString());

            var nameTask = contractHandler.QueryAsync<NameFunctionV3, string>();
            nameTask.Wait();

            var token0Task = contractHandler.QueryAsync<Token0FunctionV3, string>();
            token0Task.Wait();

            var token1Task = contractHandler.QueryAsync<Token1FunctionV3, string>();
            token1Task.Wait();

            this.OutParameters["name"].Value = nameTask.Result;
            this.OutParameters["token0"].Value = token0Task.Result;
            this.OutParameters["token1"].Value = token1Task.Result;

            return true;
        }
    }
}
