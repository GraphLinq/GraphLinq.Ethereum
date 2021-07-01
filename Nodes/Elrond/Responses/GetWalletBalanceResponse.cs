using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Elrond.Responses
{
    public class GetWalletBalanceResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("data")]
        public Data DataNode { get; set; }

        public class Account
        {
            [JsonProperty("address")]
            public string Address { get; set; }

            [JsonProperty("balance")]
            public string Balance { get; set; }

            [JsonProperty("nonce")]
            public int Nonce { get; set; }
        }

        public class Data
        {
            [JsonProperty("account")]
            public Account Account { get; set; }
        }
    }
}
