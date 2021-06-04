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
    [NodeDefinition("GetSantimentActiveWalletByCurrencyNode", "Get Santiment Active Wallets", NodeTypeEnum.Function, "Santiment")]
    [NodeGraphDescription("Get number of active wallets by currency from Santiment API")]
    [NodeIDEParameters(Hidden = true)]
    public class GetSantimentActiveWalletByCurrencyNode : Node
    {
        public GetSantimentActiveWalletByCurrencyNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetSantimentActiveWalletByCurrencyNode).Name)
        {
            this.InParameters = new Dictionary<string, NodeParameter>()
            {
                { "santiment", new NodeParameter(this, "santiment", typeof(SantimentConnector), true) },
                { "currency", new NodeParameter(this, "currency", typeof(string), true) },
            };

            this.OutParameters.Add("count", new NodeParameter(this, "count", typeof(long), false));
        }

        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var wrapperTask = _asyncWrapper();
            wrapperTask.Wait();

            this.OutParameters["count"].SetValue(wrapperTask.Result);

            return true;
        }

        private async Task<long> _asyncWrapper()
        {
            var santiment = this.InParameters["santiment"].GetValue() as SantimentConnector;
            var response = await santiment.Client.FetchNewActiveAddress(this.InParameters["currency"].GetValue().ToString(), DateTime.UtcNow.Date.AddDays(-1), DateTime.UtcNow);
            return (long)double.Parse(response.Root.NetworkGrowth.LastOrDefault().NewAddresses, CultureInfo.InvariantCulture);
        }
    }
}
