using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Ethereum.Nodes.Rarible
{
    [NodeDefinition("GetHotCollectionsNode", "Get Rarible Hot Collections", NodeTypeEnum.Function, "Rarible")]
    [NodeGraphDescription("Get hot collections on Rarible")]
    [NodeIDEParameters(Hidden = false)]
    public class GetHotCollectionsNode : Node
    {
        public GetHotCollectionsNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetHotCollectionsNode).Name)
        {
            this.InParameters.Add("rarible", new NodeParameter(this, "rarible", typeof(RaribleConnector), true));

            this.OutParameters.Add("collections", new NodeParameter(this, "collections", typeof(List<Responses.HotCollectionItem>), false));
        }

        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var wrapperTask = _asyncWrapper();
            wrapperTask.Wait();

            this.OutParameters["collections"].SetValue(wrapperTask.Result);

            return true;
        }

        private async Task<List<Responses.HotCollectionItem>> _asyncWrapper()
        {
            var api = this.InParameters["rarible"].GetValue() as RaribleConnector;
            var response = await api.WebAPI.GetHotCollections();
            return response;
        }
    }
}
