using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.Unicrypt.Entities.UniswapV2Locker;

namespace NodeBlock.Plugin.Ethereum.Nodes.Unicrypt
{
    [NodeDefinition("GetUnicryptLockedAllTokenAddress", "Get Unicrypt All LP Locked Tokens Address", NodeTypeEnum.Function, "Unicrypt")]
    [NodeGraphDescription("Get all lp address for locked tokens")]
    public class GetUnicryptLockedAllTokenAddress : Node
    {
        public GetUnicryptLockedAllTokenAddress(string id, BlockGraph graph)
            : base(id, graph, typeof(GetUnicryptLockedAllTokenAddress).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(object), true));
            this.InParameters.Add("address", new NodeParameter(this, "address", typeof(string), true));

            this.OutParameters.Add("allAddress", new NodeParameter(this, "allAddress", typeof(List<object>), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            var contractHandler = ethConnection.Web3Client.Eth.GetContractHandler(this.InParameters["address"].GetValue().ToString());

            var allAddress = new List<string>();

            var numLockTokensRequest = contractHandler.QueryAsync<GetNumLockedTokensFunction, BigInteger>(new GetNumLockedTokensFunction()
            { });
            numLockTokensRequest.Wait();
            for (var y = 0; y < (int)numLockTokensRequest.Result; y++)
            {
                var lockedTokenRequest = contractHandler.QueryAsync<GetLockedTokenAtIndexFunction, string>(new GetLockedTokenAtIndexFunction()
                {
                    Index = y
                });
                lockedTokenRequest.Wait();

                allAddress.Add(lockedTokenRequest.Result);
            }

            this.OutParameters["allAddress"].SetValue(allAddress);
            return true;
        }
    }
}
