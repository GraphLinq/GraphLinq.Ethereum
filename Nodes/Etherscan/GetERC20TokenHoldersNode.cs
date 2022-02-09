using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using AngleSharp;
using AngleSharp.Html.Parser;
using System.Net.Http;

namespace NodeBlock.Plugin.Ethereum.Nodes.Etherscan
{
    [NodeDefinition("GetERC20TokenHoldersNode", "Get ERC20 Holders", NodeTypeEnum.Function, "Blockchain.Ethereum.ERC20")]
    [NodeGraphDescription("Get the number of holders for a ERC20")]
    public class GetERC20TokenHoldersNode : Node
    {
        public GetERC20TokenHoldersNode(string id, BlockGraph graph)
         : base(id, graph, typeof(GetERC20TokenHoldersNode).Name)
        {
            this.InParameters.Add("tokenContract", new NodeParameter(this, "tokenContract", typeof(string), true));

            this.OutParameters.Add("holders", new NodeParameter(this, "holders", typeof(int), false));
        }

        private HttpClient client = new HttpClient();

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var tokenAddr = this.InParameters["tokenContract"].GetValue().ToString();
            //var response = client.GetAsync("https://etherscan.io/token/" + tokenAddr).Result;
            //var content = response.Content.ReadAsStringAsync().Result;
            //var context = BrowsingContext.New(Configuration.Default);
            //var contextTask = context.OpenAsync(req => req.Content(content));
            //contextTask.Wait();
            //var document = contextTask.Result;
            //var holderStr = document.QuerySelector("#ContentPlaceHolder1_tr_tokenHolders .mr-3").TextContent.Trim().Split(" ")[0].Replace(",", "");
            var holderStr = "0";
            
            var holders = int.Parse(holderStr);

            this.OutParameters["holders"].SetValue(holders);
            return true;
        }
    }
}
