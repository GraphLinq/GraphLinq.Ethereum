using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.Elrond.Responses;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Ethereum.Nodes.Elrond
{
    [NodeDefinition("GetElrondWalletBalanceNode", "Get Elrond Wallet Balance", NodeTypeEnum.Function, "Blockchain.Elrond")]
    [NodeGraphDescription("Get the balance of an Elrond address")]
    public class GetElrondWalletBalanceNode : Node
    {
        public GetElrondWalletBalanceNode(string id, BlockGraph graph)
          : base(id, graph, typeof(GetElrondWalletBalanceNode).Name)
        {
            this.InParameters.Add("elrond", new NodeParameter(this, "elrond", typeof(ElrondConnectorNode), true));
            this.InParameters.Add("address", new NodeParameter(this, "address", typeof(string), true));

            this.OutParameters.Add("balance", new NodeParameter(this, "balance", typeof(double), false));
            this.OutParameters.Add("nonce", new NodeParameter(this, "nonce", typeof(int), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var wrapperTask = _asyncWrapper();
            wrapperTask.Wait();

            this.OutParameters["balance"].SetValue(Web3.Convert.FromWei(BigInteger.Parse(wrapperTask.Result.DataNode.Account.Balance)));
            this.OutParameters["nonce"].SetValue(wrapperTask.Result.DataNode.Account.Nonce);

            return true;
        }

        private async Task<GetWalletBalanceResponse> _asyncWrapper()
        {
            var elrond = this.InParameters["elrond"].GetValue() as ElrondConnectorNode;
            var response = await elrond.WebAPI.FetchWalletBalance(this.InParameters["address"].GetValue().ToString());
            return response;
        }
    }
}
