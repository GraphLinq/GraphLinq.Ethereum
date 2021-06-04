using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Ethereum.Nodes.Santiment.Nodes
{
    [NodeDefinition("GetSantimentDailyVolumeNode", "Get Santiment Daily Volume", NodeTypeEnum.Function, "Santiment")]
    [NodeGraphDescription("Get daily volume by currency from Santiment API")]
    [NodeIDEParameters(Hidden = true)]
    public class GetSantimentDailyVolumeNode : Node
    {
        public GetSantimentDailyVolumeNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetSantimentDailyVolumeNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "santiment", new NodeParameter(this, "santiment", typeof(SantimentConnector), true) },
                { "currency", new NodeParameter(this, "currency", typeof(string), true) },
            };

            this.OutParameters.Add("volume", new NodeParameter(this, "volume", typeof(long), false));
        }
        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var wrapperTask = _asyncWrapper();
            wrapperTask.Wait();

            this.OutParameters["volume"].SetValue(wrapperTask.Result);

            return true;
        }

        private async Task<long> _asyncWrapper()
        {
            var santiment = this.InParameters["santiment"].GetValue() as SantimentConnector;
            var response = await santiment.Client.FetchDailyVolume(this.InParameters["currency"].GetValue().ToString(), DateTime.UtcNow.Date.AddDays(-1), DateTime.UtcNow);
            return (long)double.Parse(response.Root.GetMetric.TimeseriesData.LastOrDefault().Value, CultureInfo.InvariantCulture);
        }
    }
}
