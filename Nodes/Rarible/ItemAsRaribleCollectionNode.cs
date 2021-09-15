using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Rarible
{
    [NodeDefinition("ItemAsRaribleCollectionNode", "Item as Rarible Collection", NodeTypeEnum.Function, "Rarible")]
    [NodeGraphDescription("Convert a object to a Rarible Collection")]
    public class ItemAsRaribleCollectionNode : Node
    {
        public ItemAsRaribleCollectionNode(string id, BlockGraph graph)
            : base(id, graph, typeof(ItemAsRaribleCollectionNode).Name)
        {
            this.InParameters.Add("item", new NodeParameter(this, "item", typeof(object), true));

            this.OutParameters.Add("id", new NodeParameter(this, "id", typeof(string), false));
            this.OutParameters.Add("name", new NodeParameter(this, "name", typeof(string), false));
            this.OutParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), false));
            this.OutParameters.Add("count", new NodeParameter(this, "count", typeof(int), false));
            this.OutParameters.Add("cover", new NodeParameter(this, "cover", typeof(string), false));
            this.OutParameters.Add("pic", new NodeParameter(this, "pic", typeof(string), false));
            this.OutParameters.Add("shortUrl", new NodeParameter(this, "shortUrl", typeof(string), false));
        }

        public override bool CanBeExecuted => true;
        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var item = this.InParameters["item"].GetValue() as Responses.HotCollectionItem;

            this.OutParameters["id"].SetValue(item.Id);
            this.OutParameters["name"].SetValue(item.Name);
            this.OutParameters["symbol"].SetValue(item.Symbol);
            this.OutParameters["count"].SetValue(item.Count);
            this.OutParameters["cover"].SetValue(item.Cover);
            this.OutParameters["pic"].SetValue(item.Pic);
            this.OutParameters["shortUrl"].SetValue(item.ShortUrl);

            return true;
        }
    }
}
