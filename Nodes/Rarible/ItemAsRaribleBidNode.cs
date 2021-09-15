using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Rarible
{
    [NodeDefinition("ItemAsRaribleBidNode", "Item as Rarible Bid", NodeTypeEnum.Function, "Rarible")]
    [NodeGraphDescription("Convert a object to a Rarible Bid")]
    public class ItemAsRaribleBidNode : Node
    {
        public ItemAsRaribleBidNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ItemAsRaribleBidNode).Name)
        {
            this.InParameters.Add("item", new NodeParameter(this, "item", typeof(object), true));

            this.OutParameters.Add("orderId", new NodeParameter(this, "orderId", typeof(string), false));
            this.OutParameters.Add("buyToken", new NodeParameter(this, "buyToken", typeof(string), false));
            this.OutParameters.Add("buyTokenId", new NodeParameter(this, "buyTokenId", typeof(string), false));
            this.OutParameters.Add("buyPriceEth", new NodeParameter(this, "buyPriceEth", typeof(double), false));
            this.OutParameters.Add("createDate", new NodeParameter(this, "createDate", typeof(string), false));
            this.OutParameters.Add("id", new NodeParameter(this, "id", typeof(string), false));
        }

        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var item = this.InParameters["item"].GetValue() as Responses.HotBidItem;

            this.OutParameters["id"].SetValue(item.Id);
            this.OutParameters["orderId"].SetValue(item.OrderId);
            this.OutParameters["buyToken"].SetValue(item.BuyToken);
            this.OutParameters["buyTokenId"].SetValue(item.BuyTokenId);
            this.OutParameters["buyPriceEth"].SetValue(item.BuyPriceEth);
            this.OutParameters["createDate"].SetValue(item.CreateDate);

            return true;
        }
    }
}
