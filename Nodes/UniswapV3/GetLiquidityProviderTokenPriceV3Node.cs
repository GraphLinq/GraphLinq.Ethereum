using Nethereum.Web3;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.UniswapV3.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Numerics;
using System.Text;
//using static NodeBlock.Plugin.Ethereum.Nodes.Eth.Models.ERC20;
using static NodeBlock.Plugin.Ethereum.Nodes.UniswapV3.Entities.UniswapV3ERC20;

namespace NodeBlock.Plugin.Ethereum.Nodes.UniswapV3
{
    [NodeDefinition("GetLiquidityProviderTokenPriceV3Node", "Get Uniswap v3 LP Token Price", NodeTypeEnum.Function, "Uniswap v3")]
    [NodeGraphDescription("Return the price of the v3 LP token")]
    public class GetLiquidityProviderTokenPriceV3Node : Node
    {
        public GetLiquidityProviderTokenPriceV3Node(string id, BlockGraph graph)
           : base(id, graph, typeof(GetLiquidityProviderTokenPriceV3Node).Name)
        {
            this.InParameters.Add("ethConnection", new NodeParameter(this, "ethConnection", typeof(object), true));
            this.InParameters.Add("lpTokenAddress", new NodeParameter(this, "lpTokenAddress", typeof(string), true));

            this.OutParameters.Add("price", new NodeParameter(this, "price", typeof(double), false));
        }

        private readonly HttpClient client = new HttpClient();

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var ethConnection = this.InParameters["ethConnection"].GetValue() as EthConnection;
            var lpTokenAddress = this.InParameters["lpTokenAddress"].GetValue().ToString();

            var query = JsonConvert.SerializeObject(new
            {
                query = $"{{  bundle(id: 1) {{ ethPriceUSD }} pool(id: \"{lpTokenAddress.ToLower()}\") {{ token0Price, token1Price, totalValueLockedToken0, totalValueLockedToken1 }} }}"
            });
            var content = new StringContent(query, Encoding.UTF8, "application/json");
            var response = client.PostAsync("https://api.thegraph.com/subgraphs/name/uniswap/uniswap-v3", content).Result;
            var result = JsonConvert.DeserializeObject<UniswapPairGraphPriceV3>(response.Content.ReadAsStringAsync().Result);

            var contractHandler = ethConnection.Web3Client.Eth.GetContractHandler(lpTokenAddress);
            var supplyTask = contractHandler.QueryAsync<TotalSupplyFunctionV3, BigInteger>();
            supplyTask.Wait();
            var token0PriceUsd = double.Parse(result.Root.Bundle.EthPriceUSD, CultureInfo.InvariantCulture);
            var token1PriceUsd = double.Parse(result.Root.Pool.Token1Price, CultureInfo.InvariantCulture) * double.Parse(result.Root.Bundle.EthPriceUSD, CultureInfo.InvariantCulture);
            var lpTokenSupply = (double)Web3.Convert.FromWei(supplyTask.Result);
            var lpTokenPrice = ((token0PriceUsd * double.Parse(result.Root.Pool.TotalValueLockedToken1, CultureInfo.InvariantCulture)) + (token1PriceUsd * double.Parse(result.Root.Pool.TotalValueLockedToken0, CultureInfo.InvariantCulture))) / lpTokenSupply;

            this.OutParameters["price"].SetValue(lpTokenPrice);

            return true;
        }
    }
}
