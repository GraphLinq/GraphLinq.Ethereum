using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.Transaction
{
    [NodeDefinition("GetTransactionParametersNode", "Get Transaction Parameters", NodeTypeEnum.Function, "Blockchain.Ethereum")]
    [NodeGraphDescription("Parse the last Ethereum transaction object received into string out parameters readable")]
    public class GetTransactionParametersNode : Node
    {
        public GetTransactionParametersNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetTransactionParametersNode).Name)
        {
            this.InParameters.Add("transaction", new NodeParameter(this, "transaction", typeof(Nethereum.RPC.Eth.DTOs.Transaction), true));

            this.OutParameters.Add("etherValue", new NodeParameter(this, "etherValue", typeof(decimal), false));
            this.OutParameters.Add("from", new NodeParameter(this, "from", typeof(string), false));
            this.OutParameters.Add("to", new NodeParameter(this, "to", typeof(string), false));
            this.OutParameters.Add("gasPrice", new NodeParameter(this, "gasPrice", typeof(decimal), false));
            this.OutParameters.Add("gas", new NodeParameter(this, "gas", typeof(decimal), false));
            this.OutParameters.Add("hash", new NodeParameter(this, "hash", typeof(string), false));
            this.OutParameters.Add("blockHash", new NodeParameter(this, "blockHash", typeof(string), false));
            this.OutParameters.Add("blockNumber", new NodeParameter(this, "blockNumber", typeof(int), false));
            this.OutParameters.Add("nonce", new NodeParameter(this, "nonce", typeof(int), false));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => false;

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "gasPrice")
            {
                return Web3.Convert.FromWei((this.InParameters["transaction"].GetValue() as Nethereum.RPC.Eth.DTOs.Transaction).GasPrice);
            }
            if (parameter.Name == "etherValue")
            {
                return Web3.Convert.FromWei((this.InParameters["transaction"].GetValue() as Nethereum.RPC.Eth.DTOs.Transaction).Value);
            }
            if (parameter.Name == "hash")
            {
                return (this.InParameters["transaction"].GetValue() as Nethereum.RPC.Eth.DTOs.Transaction).TransactionHash;
            }
            if (parameter.Name == "blockHash")
            {
                return (this.InParameters["transaction"].GetValue() as Nethereum.RPC.Eth.DTOs.Transaction).BlockHash;
            }
            if (parameter.Name == "blockNumber")
            {
                return int.Parse((this.InParameters["transaction"].GetValue() as Nethereum.RPC.Eth.DTOs.Transaction).BlockNumber.ToString());
            }
            if (parameter.Name == "gas")
            {
                return Web3.Convert.FromWei((this.InParameters["transaction"].GetValue() as Nethereum.RPC.Eth.DTOs.Transaction).Gas);
            }
            if (parameter.Name == "from")
            {
                return (this.InParameters["transaction"].GetValue() as Nethereum.RPC.Eth.DTOs.Transaction).From;
            }
            if (parameter.Name == "to")
            {
                return (this.InParameters["transaction"].GetValue() as Nethereum.RPC.Eth.DTOs.Transaction).To;
            }
            if (parameter.Name == "nonce")
            {
                return int.Parse((this.InParameters["transaction"].GetValue() as Nethereum.RPC.Eth.DTOs.Transaction).Nonce.ToString());
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
