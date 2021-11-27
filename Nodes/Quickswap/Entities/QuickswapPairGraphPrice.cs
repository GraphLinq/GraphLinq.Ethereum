using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Quickswap.Entities
{
    public class QuickswapPairGraphPrice
    {
        [JsonProperty("data")]
        public Data Root { get; set; }

        public class Pair
        {
            [JsonProperty("token0Price")]
            public string Token0Price { get; set; }

            [JsonProperty("token1Price")]
            public string Token1Price { get; set; }


            [JsonProperty("reserve0")]
            public string Reserve0 { get; set; }

            [JsonProperty("reserve1")]
            public string Reserve1 { get; set; }
        }
        public class Bundle
        {
            [JsonProperty("ethPrice")]
            public string EthPrice { get; set; }
        }

        public class Data
        {
            [JsonProperty("pair")]
            public Pair Pair { get; set; }
            [JsonProperty("bundle")]
            public Bundle Bundle { get; set; }
        }

    }
}
