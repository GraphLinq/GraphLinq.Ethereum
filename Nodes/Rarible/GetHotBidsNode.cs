using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Ethereum.Nodes.Rarible
{
    [NodeDefinition("GetHotBidsNode", "Get Rarible Hot Bids", NodeTypeEnum.Function, "Rarible")]
    [NodeGraphDescription("Get hot bids on Rarible")]
    [NodeIDEParameters(Hidden = false)]
    public class GetHotBidsNode : Node
    {
        public GetHotBidsNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GetHotBidsNode).Name)
        {
            this.InParameters.Add("rarible", new NodeParameter(this, "rarible", typeof(RaribleConnector), true));
            this.InParameters.Add("size", new NodeParameter(this, "size", typeof(int), true));

            this.OutParameters.Add("bids", new NodeParameter(this, "bids", typeof(List<Responses.HotBidItem>), false));
        }

        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var wrapperTask = _asyncWrapper();
            wrapperTask.Wait();

            this.OutParameters["bids"].SetValue(wrapperTask.Result);

            return true;
        }

        private async Task<List<Responses.HotBidItem>> _asyncWrapper()
        {
            var api = this.InParameters["rarible"].GetValue() as RaribleConnector;
            var size = int.Parse(this.InParameters["size"].GetValue().ToString());
            var response = await api.WebAPI.GetHotBids(size);
            return response;
        }
    }
}
