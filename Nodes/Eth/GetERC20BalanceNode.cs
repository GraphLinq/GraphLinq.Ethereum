using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.Eth.Models.ERC20;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth
{
    [NodeDefinition("GetERC20BalanceNode", "Get ERC20 Balance", NodeTypeEnum.Function, "Blockchain.Ethereum.ERC20")]
    [NodeGraphDescription("Get the balance of an address for a ERC20")]
    public class GetERC20BalanceNode : Node
    {
        public GetERC20BalanceNode(string id, BlockGraph graph)
          : base(id, graph, typeof(GetERC20BalanceNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(object), true));
            this.InParameters.Add("address", new NodeParameter(this, "address", typeof(string), true));
            this.InParameters.Add("tokenContract", new NodeParameter(this, "tokenContract", typeof(string), true));

            this.OutParameters.Add("balance", new NodeParameter(this, "balance", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            var contractHandler = ethConnection.Web3Client.Eth.GetContractHandler(this.InParameters["tokenContract"].GetValue().ToString());
            var balanceErc20Task = contractHandler.QueryAsync<BalanceOfFunction, BigInteger>(new BalanceOfFunction()
            {
                Owner = this.InParameters["address"].GetValue().ToString(),
            });
            balanceErc20Task.Wait();
            var amount = Web3.Convert.FromWei(balanceErc20Task.Result);
            this.OutParameters["balance"].SetValue(amount);
            return true;
        }
    }
}
