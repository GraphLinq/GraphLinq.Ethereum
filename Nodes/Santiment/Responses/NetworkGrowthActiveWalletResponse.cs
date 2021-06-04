using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Santiment.Responses
{
    public class NetworkGrowthActiveWalletResponse
    {
        [JsonProperty("data")]
        public Data Root { get; set; }

        public class NetworkGrowth
        {
            [JsonProperty("datetime")]
            public DateTime Datetime { get; set; }

            [JsonProperty("newAddresses")]
            public string NewAddresses { get; set; }
        }

        public class Data
        {
            [JsonProperty("networkGrowth")]
            public List<NetworkGrowth> NetworkGrowth { get; set; }
        }
    }
}
