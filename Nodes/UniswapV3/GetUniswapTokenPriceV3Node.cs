using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.IO;
using System.Collections;
using System.Reflection;
using static NodeBlock.Plugin.Ethereum.Nodes.UniswapV3.Entities.UniswapTokenGraphPriceV3;

namespace NodeBlock.Plugin.Ethereum.Nodes.UniswapV3
{
    [NodeDefinition("GetUniswapTokenPriceV3Node", "Get Uniswap v3 Token Price", NodeTypeEnum.Function, "Uniswap v3")]
    [NodeGraphDescription("Get the price in dollar for a token on uniswap v3")]
    public class GetUniswapTokenPriceV3Node : Node
    {
        public GetUniswapTokenPriceV3Node(string id, BlockGraph graph)
            : base(id, graph, typeof(GetUniswapTokenPriceV3Node).Name)
        {
            this.InParameters.Add("token", new NodeParameter(this, "token", typeof(string), true));
            this.InParameters.Add("amount", new NodeParameter(this, "amount", typeof(double), true));

            this.OutParameters.Add("price", new NodeParameter(this, "price", typeof(double), false));
        }

        private HttpClient client = new HttpClient();

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var query = JsonConvert.SerializeObject(new
            {
                query = $"{{  bundle(id: 1) {{ ethPriceUSD }} token(id: \"{this.InParameters["token"].GetValue().ToString()}\") {{ derivedETH, name }} }}"
            });
            var content = new StringContent(query, Encoding.UTF8, "application/json");
            var response = client.PostAsync("https://api.thegraph.com/subgraphs/name/uniswap/uniswap-v3", content).Result;
            var result = JsonConvert.DeserializeObject<GraphRequestResult>(response.Content.ReadAsStringAsync().Result);

            var ethAmountInToken = double.Parse(result.Data.Token.DerivedETH, CultureInfo.InvariantCulture) * double.Parse(this.InParameters["amount"].GetValue().ToString(), CultureInfo.InvariantCulture);
            this.OutParameters["price"].Value = ethAmountInToken * double.Parse(result.Data.Bundle.EthPriceUSD, CultureInfo.InvariantCulture);

            return true;
        }
    }
}
