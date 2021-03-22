using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Uniswap.Entities
{
    public class UniswapPairGraphPrice
    {
        [JsonProperty("data")]
        public Data Root { get; set; }

        public class Pair
        {
            [JsonProperty("token0Price")]
            public string Token0Price { get; set; }

            [JsonProperty("token1Price")]
            public string Token1Price { get; set; }
        }

        public class Data
        {
            [JsonProperty("pair")]
            public Pair Pair { get; set; }
        }

    }
}
