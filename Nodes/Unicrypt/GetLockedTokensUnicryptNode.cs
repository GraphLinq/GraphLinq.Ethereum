using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.Unicrypt.Entities.UniswapV2Locker;

namespace NodeBlock.Plugin.Ethereum.Nodes.Unicrypt
{
    [NodeDefinition("GetLockedTokensUnicryptNode", "Get Unicrypt Locked Tokens", NodeTypeEnum.Function, "Unicrypt")]
    [NodeGraphDescription("Get all tokens locked in a unicrypt locker")]
    public class GetLockedTokensUnicryptNode : Node
    {
        public GetLockedTokensUnicryptNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetLockedTokensUnicryptNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(object), true));
            this.InParameters.Add("address", new NodeParameter(this, "address", typeof(string), true));
            this.InParameters.Add("lpTokenAddress", new NodeParameter(this, "lpTokenAddress", typeof(string), true));

            this.OutParameters.Add("lockedTokenArray", new NodeParameter(this, "lockedTokenArray", typeof(List<object>), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            var contractHandler = ethConnection.Web3Client.Eth.GetContractHandler(this.InParameters["address"].GetValue().ToString());

            var lockedTokenAddress = this.InParameters["lpTokenAddress"].GetValue().ToString();
            var numLockTokenRequest = contractHandler.QueryAsync<GetNumLocksForTokenFunction, BigInteger>(new GetNumLocksForTokenFunction()
            {
                LpToken = lockedTokenAddress
            });
            numLockTokenRequest.Wait();
            var objects = new List<object>();
            for (var i = 0; i < (int)numLockTokenRequest.Result; i++)
            {
                var lockedTokenRequest = contractHandler.QueryDeserializingToObjectAsync<TokenLocksFunction, TokenLocksOutputDTO>(new TokenLocksFunction()
                {
                    ReturnValue1 = lockedTokenAddress,
                    ReturnValue2 = i
                });
                lockedTokenRequest.Wait();
                var lockedToken = lockedTokenRequest.Result;
                objects.Add(lockedToken);
            }
            this.OutParameters["lockedTokenArray"].SetValue(objects);
            return true;
        }
    }
}
