using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Rarible.Responses
{
    public class HotCollectionItem
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("symbol")]
        public string Symbol { get; set; }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("sum")]
        public double Sum { get; set; }

        [JsonProperty("cover")]
        public string Cover { get; set; }

        [JsonProperty("pic")]
        public string Pic { get; set; }

        [JsonProperty("shortUrl")]
        public string ShortUrl { get; set; }
    }
}
