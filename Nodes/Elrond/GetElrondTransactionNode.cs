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
    [NodeDefinition("GetElrondTransactionNode", "Get Elrond Transaction", NodeTypeEnum.Function, "Blockchain.Elrond")]
    [NodeGraphDescription("Get a transaction on the Elrond network")]
    public class GetElrondTransactionNode : Node
    {
        public GetElrondTransactionNode(string id, BlockGraph graph)
          : base(id, graph, typeof(GetElrondTransactionNode).Name)
        {
            this.InParameters.Add("elrond", new NodeParameter(this, "elrond", typeof(ElrondConnectorNode), true));
            this.InParameters.Add("hash", new NodeParameter(this, "hash", typeof(string), true));

            this.OutParameters.Add("blockHash", new NodeParameter(this, "blockHash", typeof(string), false));
            this.OutParameters.Add("blockNonce", new NodeParameter(this, "blockNonce", typeof(int), false));
            this.OutParameters.Add("destinationShard", new NodeParameter(this, "destinationShard", typeof(int), false));
            this.OutParameters.Add("epoch", new NodeParameter(this, "epoch", typeof(int), false));
            this.OutParameters.Add("gasLimit", new NodeParameter(this, "gasLimit", typeof(long), false));
            this.OutParameters.Add("gasPrice", new NodeParameter(this, "gasPrice", typeof(int), false));
            this.OutParameters.Add("miniblockHash", new NodeParameter(this, "miniblockHash", typeof(string), false));
            this.OutParameters.Add("miniblockType", new NodeParameter(this, "miniblockType", typeof(string), false));
            this.OutParameters.Add("nonce", new NodeParameter(this, "nonce", typeof(int), false));
            this.OutParameters.Add("receiver", new NodeParameter(this, "receiver", typeof(string), false));
            this.OutParameters.Add("round", new NodeParameter(this, "round", typeof(int), false));
            this.OutParameters.Add("sender", new NodeParameter(this, "sender", typeof(string), false));
            this.OutParameters.Add("signature", new NodeParameter(this, "signature", typeof(string), false));
            this.OutParameters.Add("sourceShard", new NodeParameter(this, "sourceShard", typeof(int), false));
            this.OutParameters.Add("status", new NodeParameter(this, "status", typeof(string), false));
            this.OutParameters.Add("type", new NodeParameter(this, "type", typeof(string), false));
            this.OutParameters.Add("value", new NodeParameter(this, "value", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var wrapperTask = _asyncWrapper();
            wrapperTask.Wait();

            this.OutParameters["blockHash"].SetValue(wrapperTask.Result.Data.Transaction.BlockHash);
            this.OutParameters["blockNonce"].SetValue(wrapperTask.Result.Data.Transaction.BlockNonce);
            this.OutParameters["destinationShard"].SetValue(wrapperTask.Result.Data.Transaction.DestinationShard);
            this.OutParameters["epoch"].SetValue(wrapperTask.Result.Data.Transaction.Epoch);
            this.OutParameters["gasLimit"].SetValue(wrapperTask.Result.Data.Transaction.GasLimit);
            this.OutParameters["gasPrice"].SetValue(wrapperTask.Result.Data.Transaction.GasPrice);
            this.OutParameters["miniblockHash"].SetValue(wrapperTask.Result.Data.Transaction.MiniblockHash);
            this.OutParameters["miniblockType"].SetValue(wrapperTask.Result.Data.Transaction.MiniblockType);
            this.OutParameters["nonce"].SetValue(wrapperTask.Result.Data.Transaction.Nonce);
            this.OutParameters["receiver"].SetValue(wrapperTask.Result.Data.Transaction.Receiver);
            this.OutParameters["round"].SetValue(wrapperTask.Result.Data.Transaction.Round);
            this.OutParameters["sender"].SetValue(wrapperTask.Result.Data.Transaction.Nonce);
            this.OutParameters["signature"].SetValue(wrapperTask.Result.Data.Transaction.Signature);
            this.OutParameters["sourceShard"].SetValue(wrapperTask.Result.Data.Transaction.SourceShard);
            this.OutParameters["status"].SetValue(wrapperTask.Result.Data.Transaction.Status);
            this.OutParameters["type"].SetValue(wrapperTask.Result.Data.Transaction.Type);
            this.OutParameters["value"].SetValue(wrapperTask.Result.Data.Transaction.Value);

            return true;
        }

        private async Task<GetElrondTransactionResponse.Root> _asyncWrapper()
        {
            var elrond = this.InParameters["elrond"].GetValue() as ElrondConnectorNode;
            var response = await elrond.WebAPI.FetchTransaction(this.InParameters["hash"].GetValue().ToString());
            return response;
        }
    }
}
