using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Santiment.Nodes
{
    [NodeDefinition("SantimentConnector", "Santiment Connector", NodeTypeEnum.Connector, "Santiment")]
    [NodeGraphDescription("Setup a connector for Santiment API")]
    [NodeSpecialActionAttribute("Go to Santiment Website", "open_url", "https://app.santiment.net/")]
    [NodeIDEParameters(Hidden = true)]
    public class SantimentConnector : Node
    {
        public SantimentConnector(string id, BlockGraph graph)
            : base(id, graph, typeof(SantimentConnector).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));

            this.OutParameters.Add("santiment", new NodeParameter(this, "santiment", typeof(SantimentConnector), true));
        }

        public SantimentAPI Client;

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Client = new SantimentAPI(this.InParameters["apiKey"].GetValue().ToString());
            this.Next();
        }

        public override void OnStop()
        {
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "santiment")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
