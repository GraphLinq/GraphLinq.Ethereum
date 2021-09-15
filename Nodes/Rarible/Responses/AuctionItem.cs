using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Rarible.Responses
{
    public class AuctionItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("auction")]
        public Auction AuctionItemObject { get; set; }
        
        public class Auction
        {
            [JsonProperty("minPrice")]
            public double MinPrice { get; set; }

            [JsonProperty("currency")]
            public string Currency { get; set; }

            [JsonProperty("startDate")]
            public DateTime StartDate { get; set; }

            [JsonProperty("initEndDate")]
            public DateTime InitEndDate { get; set; }

            [JsonProperty("endDate")]
            public DateTime EndDate { get; set; }

            [JsonProperty("updateDate")]
            public DateTime UpdateDate { get; set; }

            [JsonProperty("status")]
            public string Status { get; set; }

            [JsonProperty("inProgress")]
            public bool InProgress { get; set; }

            [JsonProperty("extended")]
            public bool Extended { get; set; }
        }
    }
}
