using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Santiment.Responses
{
    public class GetDailyVolumeResponse
    {
        [JsonProperty("data")]
        public Data Root { get; set; }

        public class TimeseriesData
        {
            [JsonProperty("datetime")]
            public DateTime Datetime { get; set; }

            [JsonProperty("value")]
            public string Value { get; set; }
        }

        public class GetMetric
        {
            [JsonProperty("timeseriesData")]
            public List<TimeseriesData> TimeseriesData { get; set; }
        }

        public class Data
        {
            [JsonProperty("getMetric")]
            public GetMetric GetMetric { get; set; }
        }
    }
}
