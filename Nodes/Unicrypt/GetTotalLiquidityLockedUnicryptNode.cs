using Nethereum.Web3;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.Eth.Models;
using NodeBlock.Plugin.Ethereum.Nodes.Uniswap.Entities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Numerics;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.Eth.Models.ERC20;
using static NodeBlock.Plugin.Ethereum.Nodes.Unicrypt.Entities.UniswapV2Locker;

namespace NodeBlock.Plugin.Ethereum.Nodes.Unicrypt
{
    [NodeDefinition("GetTotalLiquidityLockedUnicryptNode", "Get Unicrypt Total Locked Liquidity", NodeTypeEnum.Function, "Unicrypt")]
    [NodeGraphDescription("Get the total liquidity locked on Unicrypt")]
    [NodeTimeout(60000 * 10)]
    public class GetTotalLiquidityLockedUnicryptNode : Node
    {
        public GetTotalLiquidityLockedUnicryptNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GetTotalLiquidityLockedUnicryptNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(object), true));
            this.InParameters.Add("address", new NodeParameter(this, "address", typeof(string), true));

            this.OutParameters.Add("totalLockedLiquidityUSD", new NodeParameter(this, "totalLockedLiquidityUSD", typeof(double), false));
            this.OutParameters.Add("pairCount", new NodeParameter(this, "pairCount", typeof(int), false));
        }
        public class UnicryptLockStat
        {
            public string tval { get; set; }
            public string pair_count { get; set; }
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        private HttpClient client = new HttpClient();

        public override bool OnExecution()
        {
            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            var contractHandler = ethConnection.Web3Client.Eth.GetContractHandler(this.InParameters["address"].GetValue().ToString());

            var totalLiquidity = 0d;
            var totalLiquidityUsd = 0d;
            //var numLockTokensRequest = contractHandler.QueryAsync<GetNumLockedTokensFunction, BigInteger>(new GetNumLockedTokensFunction()
            //{ });
            //numLockTokensRequest.Wait();
            //for(var y = 0; y < (int)numLockTokensRequest.Result; y++)
            //{
            //    var lockedTokenRequest = contractHandler.QueryAsync<GetLockedTokenAtIndexFunction, string>(new GetLockedTokenAtIndexFunction()
            //    { 
            //        Index = y
            //    });
            //    lockedTokenRequest.Wait();

            //    var numLockTokenForTokenRequest = contractHandler.QueryAsync<GetNumLocksForTokenFunction, BigInteger>(new GetNumLocksForTokenFunction()
            //    {
            //        LpToken = lockedTokenRequest.Result
            //    });
            //    numLockTokenForTokenRequest.Wait();

            //    var lpTokenAddress = lockedTokenRequest.Result;

            //    var query = JsonConvert.SerializeObject(new
            //    {
            //        query = $"{{  bundle(id: 1) {{ ethPrice }} pair(id: \"{lpTokenAddress.ToLower()}\") {{ token0Price, token1Price, reserve0, reserve1 }} }}"
            //    });
            //    var content = new StringContent(query, Encoding.UTF8, "application/json");
            //    var response = client.PostAsync("https://api.thegraph.com/subgraphs/name/uniswap/uniswap-v2", content).Result;
            //    var result = JsonConvert.DeserializeObject<UniswapPairGraphPrice>(response.Content.ReadAsStringAsync().Result);

            //    var lpTokenContractHandler = ethConnection.Web3Client.Eth.GetContractHandler(lpTokenAddress);
            //    var supplyTask = lpTokenContractHandler.QueryAsync<UniswapERC20.TotalSupplyFunctionBase, BigInteger>();
            //    supplyTask.Wait();

            //    var balanceOfContractTask = lpTokenContractHandler.QueryAsync<UniswapERC20.BalanceOfFunction, BigInteger>(new UniswapERC20.BalanceOfFunction()
            //    {
            //        ReturnValue1 = this.InParameters["address"].GetValue().ToString()
            //    });
            //    balanceOfContractTask.Wait();

            //    var token0PriceUsd = double.Parse(result.Root.Bundle.EthPrice, CultureInfo.InvariantCulture);
            //    var token1PriceUsd = double.Parse(result.Root.Pair.Token1Price, CultureInfo.InvariantCulture) * double.Parse(result.Root.Bundle.EthPrice, CultureInfo.InvariantCulture);
            //    var lpTokenSupply = (double)Web3.Convert.FromWei(supplyTask.Result);
            //    var lpTokenPrice = ((token0PriceUsd * double.Parse(result.Root.Pair.Reserve1, CultureInfo.InvariantCulture)) + (token1PriceUsd * double.Parse(result.Root.Pair.Reserve0, CultureInfo.InvariantCulture))) / lpTokenSupply;

            //    totalLiquidity += (double)Web3.Convert.FromWei(balanceOfContractTask.Result);
            //    totalLiquidityUsd += lpTokenPrice * (double)Web3.Convert.FromWei(balanceOfContractTask.Result);
            //    Console.WriteLine("LpToken: " + lpTokenAddress);
            //    Console.WriteLine("LpTokenPrice: " + lpTokenPrice);
            //    Console.WriteLine("Balance: " + (double)Web3.Convert.FromWei(balanceOfContractTask.Result));
            //    Console.WriteLine("Result: " + lpTokenPrice * (double)Web3.Convert.FromWei(balanceOfContractTask.Result));
            //}
            var request = client.GetAsync("https://unicrypt.network/api/v1/pol/lock-stats");
            request.Wait();

            var result = JsonConvert.DeserializeObject<UnicryptLockStat>(request.Result.Content.ReadAsStringAsync().Result);


            this.OutParameters["totalLockedLiquidityUSD"].SetValue(double.Parse(result.tval, CultureInfo.InvariantCulture));
            this.OutParameters["pairCount"].SetValue(int.Parse(result.pair_count));
            return true;
        }
    }
}
