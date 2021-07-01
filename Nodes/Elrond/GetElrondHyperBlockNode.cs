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
    [NodeDefinition("GetElrondHyperBlockNode", "Get Elrond HyperBlock By Hash", NodeTypeEnum.Function, "Blockchain.Elrond")]
    [NodeGraphDescription("Get hyperblock on Elrond network")]
    public class GetElrondHyperBlockNode : Node
    {
        public GetElrondHyperBlockNode(string id, BlockGraph graph)
          : base(id, graph, typeof(GetElrondHyperBlockNode).Name)
        {
            this.InParameters.Add("elrond", new NodeParameter(this, "elrond", typeof(ElrondConnectorNode), true));
            this.InParameters.Add("hash", new NodeParameter(this, "hash", typeof(string), true));

            this.OutParameters.Add("accumulatedFees", new NodeParameter(this, "accumulatedFees", typeof(string), false));
            this.OutParameters.Add("accumulatedFeesInEpoch", new NodeParameter(this, "accumulatedFeesInEpoch", typeof(string), false));
            this.OutParameters.Add("developerFees", new NodeParameter(this, "developerFees", typeof(string), false));
            this.OutParameters.Add("developerFeesInEpoch", new NodeParameter(this, "developerFeesInEpoch", typeof(string), false));
            this.OutParameters.Add("epoch", new NodeParameter(this, "epoch", typeof(int), false));
            this.OutParameters.Add("hash", new NodeParameter(this, "hash", typeof(string), false));
            this.OutParameters.Add("nonce", new NodeParameter(this, "nonce", typeof(int), false));
            this.OutParameters.Add("numTxs", new NodeParameter(this, "numTxs", typeof(int), false));
            this.OutParameters.Add("prevBlockHash", new NodeParameter(this, "prevBlockHash", typeof(string), false));
            this.OutParameters.Add("round", new NodeParameter(this, "round", typeof(int), false));
            this.OutParameters.Add("status", new NodeParameter(this, "status", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var wrapperTask = _asyncWrapper();
            wrapperTask.Wait();

            this.OutParameters["accumulatedFees"].SetValue(wrapperTask.Result.Data.Hyperblock.AccumulatedFees);
            this.OutParameters["accumulatedFeesInEpoch"].SetValue(wrapperTask.Result.Data.Hyperblock.AccumulatedFeesInEpoch);
            this.OutParameters["developerFees"].SetValue(wrapperTask.Result.Data.Hyperblock.DeveloperFees);
            this.OutParameters["developerFeesInEpoch"].SetValue(wrapperTask.Result.Data.Hyperblock.DeveloperFeesInEpoch);
            this.OutParameters["epoch"].SetValue(wrapperTask.Result.Data.Hyperblock.Epoch);
            this.OutParameters["hash"].SetValue(wrapperTask.Result.Data.Hyperblock.Hash);
            this.OutParameters["nonce"].SetValue(wrapperTask.Result.Data.Hyperblock.Nonce);
            this.OutParameters["numTxs"].SetValue(wrapperTask.Result.Data.Hyperblock.NumTxs);
            this.OutParameters["prevBlockHash"].SetValue(wrapperTask.Result.Data.Hyperblock.PrevBlockHash);
            this.OutParameters["round"].SetValue(wrapperTask.Result.Data.Hyperblock.Round);
            this.OutParameters["status"].SetValue(wrapperTask.Result.Data.Hyperblock.Status);

            return true;
        }

        private async Task<GetElrondHyperBlockResponse.Root> _asyncWrapper()
        {
            var elrond = this.InParameters["elrond"].GetValue() as ElrondConnectorNode;
            var response = await elrond.WebAPI.FetchHyperBlockByHash(this.InParameters["hash"].GetValue().ToString());
            return response;
        }
    }
}
