using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Elrond
{
    [NodeDefinition("ElrondConnectorNode", "Elrond Connector", NodeTypeEnum.Connector, "Blockchain.Elrond")]
    [NodeGraphDescription("Connection to the Elrond network")]
    public class ElrondConnectorNode : Node
    {
        public ElrondConnectorNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ElrondConnectorNode).Name)
        {
            this.CanBeSerialized = false;
            this.OutParameters.Add("elrond", new NodeParameter(this, "elrond", typeof(ElrondConnectorNode), true));
        }

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public ElrondWebAPI WebAPI { get; set; }


        public override void SetupConnector()
        {
            this.WebAPI = new ElrondWebAPI();
            this.Next();
        }

        public override void OnStop()
        {

        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "elrond")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
