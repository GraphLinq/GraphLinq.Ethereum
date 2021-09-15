using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Ethereum.Nodes.Rarible
{
    [NodeDefinition("GetAuctionsNode", "Get Rarible Auctions", NodeTypeEnum.Function, "Rarible")]
    [NodeGraphDescription("Get auctions on Rarible")]
    [NodeIDEParameters(Hidden = false)]
    public class GetAuctionsNode : Node
    {
        public GetAuctionsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetAuctionsNode).Name)
        {
            this.InParameters.Add("rarible", new NodeParameter(this, "rarible", typeof(RaribleConnector), true));

            this.OutParameters.Add("auctions", new NodeParameter(this, "auctions", typeof(List<Responses.AuctionItem>), false));
        }

        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var wrapperTask = _asyncWrapper();
            wrapperTask.Wait();

            this.OutParameters["auctions"].SetValue(wrapperTask.Result);

            return true;
        }

        private async Task<List<Responses.AuctionItem>> _asyncWrapper()
        {
            var api = this.InParameters["rarible"].GetValue() as RaribleConnector;
            var response = await api.WebAPI.GetAuctions();
            return response;
        }
    }
}
