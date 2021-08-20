using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.NftCreator.Models
{
    public class NFTTokenModel
    {
        public string ContractAddress { get; set; }
        public string TokenOwner { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }

    }
}
