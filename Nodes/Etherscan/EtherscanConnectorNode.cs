using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Etherscan
{
    [NodeDefinition("EtherscanConnectorNode", "Etherscan Connector", NodeTypeEnum.Connector, "Blockchain.Ethereum")]
    [NodeGraphDescription("Connection to the Etherscan API")]
    public class EtherscanConnectorNode : Node
    {
        public EtherscanConnectorNode(string id, BlockGraph graph)
          : base(id, graph, typeof(EtherscanConnectorNode).Name)
        {
            this.InParameters.Add("apiKey", new NodeParameter(this, "apiKey", typeof(string), true));

            this.OutParameters.Add("etherscanConnection", new NodeParameter(this, "etherscanConnection", typeof(EtherscanConnectorNode), true));
        }


        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            this.Next();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "etherscanConnection")
            {
                return this;
            }

            return base.ComputeParameterValue(parameter, value);
        }
    }
}
