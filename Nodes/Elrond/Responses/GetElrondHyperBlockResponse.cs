using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Elrond.Responses
{
    public class GetElrondHyperBlockResponse
    {
        public class ShardBlock
        {
            [JsonProperty("hash")]
            public string Hash { get; set; }

            [JsonProperty("nonce")]
            public int Nonce { get; set; }

            [JsonProperty("shard")]
            public int Shard { get; set; }
        }

        public class Transaction
        {
            [JsonProperty("data")]
            public string Data { get; set; }

            [JsonProperty("destinationShard")]
            public long DestinationShard { get; set; }

            [JsonProperty("gasLimit")]
            public int GasLimit { get; set; }

            [JsonProperty("gasPrice")]
            public int GasPrice { get; set; }

            [JsonProperty("hash")]
            public string Hash { get; set; }

            [JsonProperty("miniblockHash")]
            public string MiniblockHash { get; set; }

            [JsonProperty("miniblockType")]
            public string MiniblockType { get; set; }

            [JsonProperty("nonce")]
            public int Nonce { get; set; }

            [JsonProperty("receiver")]
            public string Receiver { get; set; }

            [JsonProperty("sender")]
            public string Sender { get; set; }

            [JsonProperty("signature")]
            public string Signature { get; set; }

            [JsonProperty("sourceShard")]
            public long SourceShard { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("type")]
            public string Type { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }

            [JsonProperty("originalTransactionHash")]
            public string OriginalTransactionHash { get; set; }

            [JsonProperty("previousTransactionHash")]
            public string PreviousTransactionHash { get; set; }
        }

        public class Hyperblock
        {
            [JsonProperty("accumulatedFees")]
            public string AccumulatedFees { get; set; }

            [JsonProperty("accumulatedFeesInEpoch")]
            public string AccumulatedFeesInEpoch { get; set; }

            [JsonProperty("developerFees")]
            public string DeveloperFees { get; set; }

            [JsonProperty("developerFeesInEpoch")]
            public string DeveloperFeesInEpoch { get; set; }

            [JsonProperty("epoch")]
            public int Epoch { get; set; }

            [JsonProperty("hash")]
            public string Hash { get; set; }

            [JsonProperty("nonce")]
            public int Nonce { get; set; }

            [JsonProperty("numTxs")]
            public int NumTxs { get; set; }

            [JsonProperty("prevBlockHash")]
            public string PrevBlockHash { get; set; }

            [JsonProperty("round")]
            public int Round { get; set; }

            [JsonProperty("shardBlocks")]
            public List<ShardBlock> ShardBlocks { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("transactions")]
            public List<Transaction> Transactions { get; set; }
        }

        public class Data
        {
            [JsonProperty("hyperblock")]
            public Hyperblock Hyperblock { get; set; }
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
