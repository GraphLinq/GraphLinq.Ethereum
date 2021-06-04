using Newtonsoft.Json;
using NodeBlock.Plugin.Ethereum.Nodes.Santiment.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Ethereum.Nodes.Santiment
{
    public class SantimentAPI
    {
        public string APIKey { get; }
        private HttpClient client = new HttpClient();
        private string baseUrl = "https://api.santiment.net/graphql";

        public SantimentAPI(string apiKey)
        {
            APIKey = apiKey;
            this.client.DefaultRequestHeaders.Add("Authorization", "Apikey " + this.APIKey);
        }

        public async Task<NetworkGrowthActiveWalletResponse> FetchNewActiveAddress(string slug, DateTime from, DateTime to)
        {
            var query = @$"{{ 
                networkGrowth(from: ""{from.ToString("yyyy-MM-ddTHH:mm:ssZ")}"", interval: ""1d"", slug: ""{slug}"", to: ""{to.ToString("yyyy-MM-ddTHH:mm:ssZ")}"")
                {{
                    newAddresses
                    datetime
                }}
            }}";
            var request = await client.PostAsync(baseUrl, new StringContent(query, Encoding.UTF8, "application/graphql"));
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<NetworkGrowthActiveWalletResponse>(responseContent);
            return data;
        }

        public async Task<GetDailyVolumeResponse> FetchDailyVolume(string slug, DateTime from, DateTime to)
        {
            var query = @$"{{
                  getMetric(metric: ""volume_usd"") {{
                    timeseriesData(
                      slug: ""{slug}""
                      from: ""{from.ToString("yyyy-MM-ddTHH:mm:ssZ")}""
                      to: ""{to.ToString("yyyy-MM-ddTHH:mm:ssZ")}""
                      includeIncompleteData: false
                      interval: ""1d""
                    ) {{
                      datetime
                      value
                    }}
                  }}
                }}";
            var request = await client.PostAsync(baseUrl, new StringContent(query, Encoding.UTF8, "application/graphql"));
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<GetDailyVolumeResponse>(responseContent);
            return data;
        }

        public async Task<GetDailyVolumeResponse> FetchDailyTransaction(string slug, DateTime from, DateTime to)
        {
            var query = @$"{{
                  getMetric(metric: ""transactions_count"") {{
                    timeseriesData(
                      slug: ""{slug}""
                      from: ""{from.ToString("yyyy-MM-ddTHH:mm:ssZ")}""
                      to: ""{to.ToString("yyyy-MM-ddTHH:mm:ssZ")}""
                      includeIncompleteData: true
                      interval: ""1d""
                    ) {{
                      datetime
                      value
                    }}
                  }}
                }}";
            var request = await client.PostAsync(baseUrl, new StringContent(query, Encoding.UTF8, "application/graphql"));
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<GetDailyVolumeResponse>(responseContent);
            return data;
        }
    }
}
