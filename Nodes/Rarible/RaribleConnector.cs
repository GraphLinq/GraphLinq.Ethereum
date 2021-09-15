using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Rarible
{
    [NodeDefinition("RaribleConnector", "Rarible Connector", NodeTypeEnum.Connector, "Rarible")]
    [NodeGraphDescription("Connection to the Rarible network")]
    public class RaribleConnector : Node
    {
        public RaribleConnector(string id, BlockGraph graph)
            : base(id, graph, typeof(RaribleConnector).Name)
        {
            this.CanBeSerialized = false;
            this.OutParameters.Add("rarible", new NodeParameter(this, "rarible", typeof(RaribleConnector), true));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public RaribleAPI WebAPI { get; set; }


        public override void SetupConnector()
        {
            this.WebAPI = new RaribleAPI();
            this.Next();
        }

        public override void OnStop()
        {

        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "rarible")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
