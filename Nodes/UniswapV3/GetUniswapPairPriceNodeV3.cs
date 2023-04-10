using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.UniswapV3.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.UniswapV3
{
    [NodeDefinition("GetUniswapPairPriceV3Node", "Get Uniswap v3 Pair Price", NodeTypeEnum.Function, "Uniswap v3")]
    [NodeGraphDescription("Return the pair price of an Uniswap v3 pool as out parameters")]
    public class GetUniswapPairPriceV3Node : Node
    {
        public GetUniswapPairPriceV3Node(string id, BlockGraph graph)
          : base(id, graph, typeof(GetUniswapPairPriceV3Node).Name)
        {
            this.InParameters.Add("pairAddress", new NodeParameter(this, "pairAddress", typeof(string), true));

            this.OutParameters.Add("token0Price", new NodeParameter(this, "token0Price", typeof(double), false));
            this.OutParameters.Add("token1Price", new NodeParameter(this, "token1Price", typeof(double), false));
        }

        private HttpClient client = new HttpClient();

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var query = JsonConvert.SerializeObject(new
            {
                query = $"{{ pool(id: \"{this.InParameters["pairAddress"].GetValue().ToString()}\") {{ token0Price, token1Price }} }}"
            });
            var content = new StringContent(query, Encoding.UTF8, "application/json");
            var response = client.PostAsync("https://api.thegraph.com/subgraphs/name/uniswap/uniswap-v3", content).Result;
            var result = JsonConvert.DeserializeObject<UniswapPairGraphPriceV3>(response.Content.ReadAsStringAsync().Result);

            this.OutParameters["token0Price"].SetValue(double.Parse(result.Root.Pool.Token0Price, CultureInfo.InvariantCulture));
            this.OutParameters["token1Price"].SetValue(double.Parse(result.Root.Pool.Token1Price, CultureInfo.InvariantCulture));

            return true;
        }
    }
}
