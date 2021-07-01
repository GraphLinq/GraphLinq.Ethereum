using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Elrond.Responses
{
    public class GetElrondTransactionResponse
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse); 
        public class Transaction
        {
            [JsonProperty("blockHash")]
            public string BlockHash { get; set; }

            [JsonProperty("blockNonce")]
            public int BlockNonce { get; set; }

            [JsonProperty("data")]
            public string Data { get; set; }

            [JsonProperty("destinationShard")]
            public int DestinationShard { get; set; }

            [JsonProperty("epoch")]
            public int Epoch { get; set; }

            [JsonProperty("gasLimit")]
            public int GasLimit { get; set; }

            [JsonProperty("gasPrice")]
            public int GasPrice { get; set; }

            [JsonProperty("miniblockHash")]
            public string MiniblockHash { get; set; }

            [JsonProperty("miniblockType")]
            public string MiniblockType { get; set; }

            [JsonProperty("nonce")]
            public int Nonce { get; set; }

            [JsonProperty("receiver")]
            public string Receiver { get; set; }

            [JsonProperty("round")]
            public int Round { get; set; }

            [JsonProperty("sender")]
            public string Sender { get; set; }

            [JsonProperty("signature")]
            public string Signature { get; set; }

            [JsonProperty("sourceShard")]
            public int SourceShard { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }

        public class Data
        {
            [JsonProperty("transaction")]
            public Transaction Transaction { get; set; }
        }

        public class Root
        {
            [JsonProperty("code")]
            public string Code { get; set; }

            [JsonProperty("data")]
            public Data Data { get; set; }
        }
    }
}
