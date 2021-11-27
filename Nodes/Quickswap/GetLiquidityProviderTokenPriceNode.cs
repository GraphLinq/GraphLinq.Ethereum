using Nethereum.Web3;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.Quickswap.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Numerics;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.Eth.Models.ERC20;

namespace NodeBlock.Plugin.Ethereum.Nodes.Quickswap
{
    [NodeDefinition("GetLiquidityProviderTokenPriceNode", "Get Quickswap LP Token Price", NodeTypeEnum.Function, "Quickswap")]
    [NodeGraphDescription("Return the price of the LP token")]
    [NodeIDEParameters(Hidden = true)]
    public class GetLiquidityProviderTokenPriceNode : Node
    {
        public GetLiquidityProviderTokenPriceNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GetLiquidityProviderTokenPriceNode).Name)
        {
            this.InParameters.Add("ethConnection", new NodeParameter(this, "ethConnection", typeof(object), true));
            this.InParameters.Add("lpTokenAddress", new NodeParameter(this, "lpTokenAddress", typeof(string), true));

            this.OutParameters.Add("price", new NodeParameter(this, "price", typeof(double), false));
        }

        private HttpClient client = new HttpClient();

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var ethConnection = this.InParameters["ethConnection"].GetValue() as EthConnection;
            var lpTokenAddress = this.InParameters["lpTokenAddress"].GetValue().ToString();

            var query = JsonConvert.SerializeObject(new
            {
                query = $"{{  bundle(id: 1) {{ ethPrice }} pair(id: \"{lpTokenAddress.ToLower()}\") {{ token0Price, token1Price, reserve0, reserve1 }} }}"
            });
            var content = new StringContent(query, Encoding.UTF8, "application/json");
            var response = client.PostAsync("https://api.thegraph.com/subgraphs/name/sameepsi/quickswap06", content).Result;
            var result = JsonConvert.DeserializeObject<QuickswapPairGraphPrice>(response.Content.ReadAsStringAsync().Result);

            var contractHandler = ethConnection.Web3Client.Eth.GetContractHandler(lpTokenAddress);
            var supplyTask = contractHandler.QueryAsync<TotalSupplyFunction, BigInteger>();
            supplyTask.Wait();

            var token0PriceUsd = double.Parse(result.Root.Bundle.EthPrice, CultureInfo.InvariantCulture);
            var token1PriceUsd = double.Parse(result.Root.Pair.Token1Price, CultureInfo.InvariantCulture) * double.Parse(result.Root.Bundle.EthPrice, CultureInfo.InvariantCulture);
            var lpTokenSupply = (double)Web3.Convert.FromWei(supplyTask.Result);
            var lpTokenPrice = ((token0PriceUsd * double.Parse(result.Root.Pair.Reserve1, CultureInfo.InvariantCulture)) + (token1PriceUsd * double.Parse(result.Root.Pair.Reserve0, CultureInfo.InvariantCulture))) / lpTokenSupply;

            this.OutParameters["price"].SetValue(lpTokenPrice);

            return true;
        }
    }
}
