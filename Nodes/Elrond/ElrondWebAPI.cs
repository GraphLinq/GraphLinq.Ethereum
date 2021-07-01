using Newtonsoft.Json;
using NodeBlock.Plugin.Ethereum.Nodes.Elrond.Responses;
using NodeBlock.Plugin.Ethereum.Nodes.Santiment.Responses;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Ethereum.Nodes.Elrond
{
    public class ElrondWebAPI
    {
        private HttpClient client = new HttpClient();
        private string baseUrl = "https://api.elrond.com";

        public async Task<GetWalletBalanceResponse> FetchWalletBalance(string addr)
        {
            var request = await client.GetAsync(baseUrl + "/address/" + addr);
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<GetWalletBalanceResponse>(responseContent);
            return data;
        }

        public async Task<GetElrondTransactionResponse.Root> FetchTransaction(string hash)
        {
            var request = await client.GetAsync(baseUrl + "/transaction/" + hash);
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<GetElrondTransactionResponse.Root>(responseContent);
            return data;
        }

        public async Task<GetElrondHyperBlockResponse.Root> FetchHyperBlockByHash(string hash)
        {
            var request = await client.GetAsync(baseUrl + "/hyperblock/by-hash/" + hash);
            var responseContent = await request.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<GetElrondHyperBlockResponse.Root>(responseContent);
            return data;
        }
    }
}
