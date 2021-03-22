using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Unicrypt
{

    //[NodeDefinition("GetUnicryptPresaleNode", "Get Unicrypt Presale (ETH)", NodeTypeEnum.Function, "Unicrypt")]
    //[NodeGraphDescription("Get all informations from a Unicrypt presale")]
    public class GetUnicryptPresaleNode : Node
    {
        public GetUnicryptPresaleNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetUnicryptPresaleNode).Name)
        {
            this.IsEventNode = true;

            this.InParameters.Add("ethereumConnection", new NodeParameter(this, "ethereumConnection", typeof(object), true));
            this.InParameters.Add("bscConnection", new NodeParameter(this, "bscConnection", typeof(object), true));
            this.InParameters.Add("contractAddress", new NodeParameter(this, "contractAddress", typeof(string), true));

        }

        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;

        public override bool OnExecution()
        {



            return true;
        }
    }
}
