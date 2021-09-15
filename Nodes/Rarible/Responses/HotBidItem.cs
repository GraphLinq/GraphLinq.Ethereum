using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Rarible.Responses
{
    public class HotBidItem
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("buyToken")]
        public string BuyToken { get; set; }

        [JsonProperty("buyTokenId")]
        public string BuyTokenId { get; set; }

        [JsonProperty("buyPriceEth")]
        public double BuyPriceEth { get; set; }

        [JsonProperty("createDate")]
        public DateTime CreateDate { get; set; }

        [JsonProperty("categories")]
        public List<string> Categories { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
