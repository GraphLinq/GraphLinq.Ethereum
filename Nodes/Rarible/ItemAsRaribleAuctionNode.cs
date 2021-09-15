using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Rarible
{
    [NodeDefinition("ItemAsRaribleAuctionNode", "Item as Rarible Auction", NodeTypeEnum.Function, "Rarible")]
    [NodeGraphDescription("Convert a object to a Rarible Auction")]
    public class ItemAsRaribleAuctionNode : Node
    {
        public ItemAsRaribleAuctionNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ItemAsRaribleAuctionNode).Name)
        {
            this.InParameters.Add("item", new NodeParameter(this, "item", typeof(object), true));

            this.OutParameters.Add("id", new NodeParameter(this, "id", typeof(string), false));
            this.OutParameters.Add("minPrice", new NodeParameter(this, "minPrice", typeof(double), false));
            this.OutParameters.Add("currency", new NodeParameter(this, "currency", typeof(string), false));
            this.OutParameters.Add("startDate", new NodeParameter(this, "startDate", typeof(string), false));
            this.OutParameters.Add("initEndDate", new NodeParameter(this, "initEndDate", typeof(string), false));
            this.OutParameters.Add("endDate", new NodeParameter(this, "endDate", typeof(string), false));
            this.OutParameters.Add("updateDate", new NodeParameter(this, "updateDate", typeof(string), false));
            this.OutParameters.Add("status", new NodeParameter(this, "status", typeof(string), false));
            this.OutParameters.Add("inProgress", new NodeParameter(this, "inProgress", typeof(bool), false));
            this.OutParameters.Add("extended", new NodeParameter(this, "extended", typeof(bool), false));
        }

        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var item = this.InParameters["item"].GetValue() as Responses.AuctionItem;

            this.OutParameters["id"].SetValue(item.Id);
            this.OutParameters["minPrice"].SetValue(item.AuctionItemObject.MinPrice);
            this.OutParameters["currency"].SetValue(item.AuctionItemObject.Currency);
            this.OutParameters["startDate"].SetValue(item.AuctionItemObject.StartDate);
            this.OutParameters["initEndDate"].SetValue(item.AuctionItemObject.InitEndDate);
            this.OutParameters["endDate"].SetValue(item.AuctionItemObject.EndDate);
            this.OutParameters["updateDate"].SetValue(item.AuctionItemObject.UpdateDate);
            this.OutParameters["status"].SetValue(item.AuctionItemObject.Status);
            this.OutParameters["inProgress"].SetValue(item.AuctionItemObject.InProgress);
            this.OutParameters["extended"].SetValue(item.AuctionItemObject.Extended);

            return true;
        }
    }
}
