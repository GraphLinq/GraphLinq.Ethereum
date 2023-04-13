using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.UniswapV3.Entities
{
    public class UniswapPairGraphPriceV3
    {
        [JsonProperty("data")]
        public Data Root { get; set; }

        public class Pool
        {
            [JsonProperty("token0Price")]
            public string Token0Price { get; set; }

            [JsonProperty("token1Price")]
            public string Token1Price { get; set; }

            [JsonProperty("totalValueLockedToken0")]
            public string TotalValueLockedToken0 { get; set; }

            [JsonProperty("totalValueLockedToken1")]
            public string TotalValueLockedToken1 { get; set; }
        }
        public class Bundle
        {
            [JsonProperty("ethPriceUSD")]
            public string EthPriceUSD { get; set; }
        }

        public class Data
        {
            [JsonProperty("bundle")]
            public Bundle Bundle { get; set; }

            [JsonProperty("pool")]
            public Pool Pool { get; set; }

        }

    }
}
