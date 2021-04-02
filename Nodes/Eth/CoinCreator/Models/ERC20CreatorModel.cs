using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.CoinCreator.Models
{
    public class ERC20CreatorModel
    {
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string Owner { get; set; }
        public BigInteger MaxSupply { get; set; }
        public BigInteger InitialSupply { get; set; }
    }
}
