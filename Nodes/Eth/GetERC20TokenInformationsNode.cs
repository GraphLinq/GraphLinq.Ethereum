using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.Eth.Models.ERC20;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth
{
    [NodeDefinition("GetERC20TokenInformationsNode", "Get ERC20 Informations", NodeTypeEnum.Function, "Blockchain.Ethereum.ERC20")]
    [NodeGraphDescription("Get informations about a ERC20 token")]
    public class GetERC20TokenInformationsNode : Node
    {
        public GetERC20TokenInformationsNode(string id, BlockGraph graph)
         : base(id, graph, typeof(GetERC20TokenInformationsNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(object), true));
            this.InParameters.Add("tokenContract", new NodeParameter(this, "tokenContract", typeof(string), true));

            this.OutParameters.Add("totalSupply", new NodeParameter(this, "totalSupply", typeof(double), false));
            this.OutParameters.Add("name", new NodeParameter(this, "name", typeof(string), false));
            this.OutParameters.Add("symbol", new NodeParameter(this, "symbol", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            var contractHandler = ethConnection.Web3Client.Eth.GetContractHandler(this.InParameters["tokenContract"].GetValue().ToString());
            var totalSupplyTask = contractHandler.QueryAsync<TotalSupplyFunction, BigInteger>(new TotalSupplyFunction());
            totalSupplyTask.Wait();
            this.OutParameters["totalSupply"].SetValue(Web3.Convert.FromWei(totalSupplyTask.Result));

            var nameTask = contractHandler.QueryAsync<NameFunction, string>(new NameFunction());
            nameTask.Wait();
            this.OutParameters["name"].SetValue(nameTask.Result);

            var symbolTask = contractHandler.QueryAsync<SymbolFunction, string>(new SymbolFunction());
            symbolTask.Wait();
            this.OutParameters["symbol"].SetValue(symbolTask.Result);
            return true;
        }
    }
}
