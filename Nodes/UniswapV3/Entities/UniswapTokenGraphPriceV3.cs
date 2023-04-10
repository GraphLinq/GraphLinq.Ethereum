using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.UniswapV3.Entities
{
    public class UniswapTokenGraphPriceV3
    {
        public class Bundle
        {
            [JsonProperty("ethPriceUSD")]
            public string EthPriceUSD { get; set; }
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

    }
}
