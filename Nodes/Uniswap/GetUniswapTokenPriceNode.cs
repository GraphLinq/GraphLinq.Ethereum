using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Uniswap
{
    [NodeDefinition("GetUniswapTokenPriceNode", "Get Uniswap Token Price", NodeTypeEnum.Function, "Uniswap")]
    [NodeGraphDescription("Get the price in dollar for a token on uniswap")]
    public class GetUniswapTokenPriceNode : Node
    {
        public class Bundle
        {
            [JsonProperty("ethPrice")]
            public string EthPrice { get; set; }
        }

        public class Token
        {
            [JsonProperty("derivedETH")]
            public string DerivedETH { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }
        }

        public class Data
        {
            [JsonProperty("bundle")]
            public Bundle Bundle { get; set; }

            [JsonProperty("token")]
            public Token Token { get; set; }
        }

        public class GraphRequestResult
        {
            [JsonProperty("data")]
            public Data Data { get; set; }
        }

        public GetUniswapTokenPriceNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetUniswapTokenPriceNode).Name)
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
                query = $"{{  bundle(id: 1) {{ ethPrice }} token(id: \"{this.InParameters["token"].GetValue().ToString()}\") {{ derivedETH }} }}"
            });
            var content = new StringContent(query, Encoding.UTF8, "application/json");
            var response = client.PostAsync("https://api.thegraph.com/subgraphs/name/uniswap/uniswap-v2", content).Result;
            var result = JsonConvert.DeserializeObject<GraphRequestResult>(response.Content.ReadAsStringAsync().Result);

            var ethAmountInToken = double.Parse(result.Data.Token.DerivedETH, CultureInfo.InvariantCulture) * double.Parse(this.InParameters["amount"].GetValue().ToString(), CultureInfo.InvariantCulture);
            this.OutParameters["price"].Value = ethAmountInToken * double.Parse(result.Data.Bundle.EthPrice, CultureInfo.InvariantCulture);

            return true;
        }
    }
}
