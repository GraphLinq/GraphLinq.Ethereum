using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth
{
    [NodeDefinition("GetEtherBalanceNode", "Get Ether Balance", NodeTypeEnum.Function, "Blockchain.Ethereum")]
    [NodeGraphDescription("Get the balance of address in ether")]
    public class GetEtherBalanceNode : Node
    {
        public GetEtherBalanceNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetEtherBalanceNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(object), true));
            this.InParameters.Add("address", new NodeParameter(this, "address", typeof(string), true));

            this.OutParameters.Add("balance", new NodeParameter(this, "balance", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            var request = ethConnection.Web3Client.Eth.GetBalance.SendRequestAsync(this.InParameters["address"].GetValue().ToString());
            request.Wait();
            var etherAmount = Web3.Convert.FromWei(request.Result.Value);
            this.OutParameters["balance"].SetValue(etherAmount);
            return true;
        }
    }
}
