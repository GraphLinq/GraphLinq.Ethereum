using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Ethereum.Nodes.Rarible
{
    public class RaribleAPI
    {
        private HttpClient client = new HttpClient();
        private string baseUrl = "https://api-mainnet.rarible.com";

        public RaribleAPI()
        {

        }

        public async Task<List<Responses.HotCollectionItem>> GetHotCollections()
        {
            var request = await client.GetAsync(baseUrl + "/marketplace/api/v4/collections/hot");
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Responses.HotCollectionItem>>(responseContent);
            return data;
        }

        public async Task<List<Responses.AuctionItem>> GetAuctions()
        {
            var request = await client.GetAsync(baseUrl + "marketplace/api/v4/auctions");
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Responses.AuctionItem>>(responseContent);
            return data;
        }

        public async Task<List<Responses.HotBidItem>> GetHotBids(int size)
        {
            StringContent httpContent = new StringContent(JsonConvert.SerializeObject(new
            {
                size = size
            }), System.Text.Encoding.UTF8, "application/json");
            var request = await client.PostAsync(baseUrl + "/marketplace/api/v4/hotBids", httpContent);
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<List<Responses.HotBidItem>>(responseContent);
            return data;
        }
    }
}
