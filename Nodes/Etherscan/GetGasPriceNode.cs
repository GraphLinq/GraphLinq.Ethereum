using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Etherscan
{
    [NodeDefinition("GetGasPriceNode", "Etherscan Get Gas Price", NodeTypeEnum.Function, "Blockchain.Ethereum")]
    [NodeGraphDescription("Return the current GAS Price on the Ethereum blockchain as out parameter")]
    [NodeGasConfiguration("10000000000000")]
    public class GetGasPriceNode : Node
    {
        public class GasGasPriceHttpResult
        {
            public class Result
            {
                public int LastBlock { get; set; }
                public int SafeGasPrice { get; set; }
                public int ProposeGasPrice { get; set; }
                public int FastGasPrice { get; set; }
            }
            public string status { get; set; }
            public string message { get; set; }
            public Result result { get; set; }
        }

        private HttpClient client = new HttpClient();

        public GetGasPriceNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GetGasPriceNode).Name)
        {
            this.InParameters.Add("etherscanConnection", new NodeParameter(this, "etherscanConnection", typeof(EtherscanConnectorNode), true));

            this.OutParameters.Add("lastBlock", new NodeParameter(this, "lastBlock", typeof(int), false));
            this.OutParameters.Add("safeGasPrice", new NodeParameter(this, "safeGasPrice", typeof(int), false));
            this.OutParameters.Add("proposeGasPrice", new NodeParameter(this, "proposeGasPrice", typeof(int), false));
            this.OutParameters.Add("fastGasPrice", new NodeParameter(this, "fastGasPrice", typeof(int), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var apiKey = (this.InParameters["etherscanConnection"].GetValue() as EtherscanConnectorNode).InParameters["apiKey"].GetValue();
            var response = client.GetAsync("https://api.etherscan.io/api?module=gastracker&action=gasoracle&apikey=" + apiKey).Result;
            var gasPriceResponse = JsonConvert.DeserializeObject<GasGasPriceHttpResult>(response.Content.ReadAsStringAsync().Result);

            this.OutParameters["lastBlock"].SetValue(gasPriceResponse.result.LastBlock);
            this.OutParameters["safeGasPrice"].SetValue(gasPriceResponse.result.SafeGasPrice);
            this.OutParameters["proposeGasPrice"].SetValue(gasPriceResponse.result.ProposeGasPrice);
            this.OutParameters["fastGasPrice"].SetValue(gasPriceResponse.result.FastGasPrice);

            return true;
        }
    }
}
