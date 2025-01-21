using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.GraphLinq
{
    [NodeDefinition("GetGlqBalanceNode", "Get GLQ Balance", NodeTypeEnum.Function, "Blockchain.GraphLinq")]
    [NodeGraphDescription("Get the balance of address in GLQ")]
    public class GetGlqBalanceNode : Node
    {
        public GetGlqBalanceNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetGlqBalanceNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(object), true));
            this.InParameters.Add("address", new NodeParameter(this, "address", typeof(string), true));

            this.OutParameters.Add("balance", new NodeParameter(this, "balance", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            GlqConnection glqConnection = this.InParameters["connection"].GetValue() as GlqConnection;
            var request = glqConnection.Web3Client.Eth.GetBalance.SendRequestAsync(this.InParameters["address"].GetValue().ToString());
            request.Wait();
            var etherAmount = Web3.Convert.FromWei(request.Result.Value);
            this.OutParameters["balance"].SetValue(etherAmount);
            return true;
        }
    }
}
