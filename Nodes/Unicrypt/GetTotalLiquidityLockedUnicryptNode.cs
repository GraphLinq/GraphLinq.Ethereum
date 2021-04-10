using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.Unicrypt.Entities.UniswapV2Locker;

namespace NodeBlock.Plugin.Ethereum.Nodes.Unicrypt
{
    [NodeDefinition("GetTotalLiquidityLockedUnicryptNode", "Get Unicrypt Total Locked Liquidity", NodeTypeEnum.Function, "Unicrypt")]
    [NodeGraphDescription("Get the total liquidity locked on Unicrypt")]
    public class GetTotalLiquidityLockedUnicryptNode : Node
    {
        public GetTotalLiquidityLockedUnicryptNode(string id, BlockGraph graph)
           : base(id, graph, typeof(GetTotalLiquidityLockedUnicryptNode).Name)
        {
            this.InParameters.Add("connection", new NodeParameter(this, "connection", typeof(object), true));
            this.InParameters.Add("address", new NodeParameter(this, "address", typeof(string), true));

            this.OutParameters.Add("totalLockedLiquidity", new NodeParameter(this, "totalLockedLiquidity", typeof(double), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            EthConnection ethConnection = this.InParameters["connection"].GetValue() as EthConnection;
            var contractHandler = ethConnection.Web3Client.Eth.GetContractHandler(this.InParameters["address"].GetValue().ToString());

            var totalLiquidity = 0d;
            var numLockTokensRequest = contractHandler.QueryAsync<GetNumLockedTokensFunction, BigInteger>(new GetNumLockedTokensFunction()
            { });
            numLockTokensRequest.Wait();
            for(var y = 0; y < (int)numLockTokensRequest.Result; y++)
            {
                var lockedTokenRequest = contractHandler.QueryAsync<GetLockedTokenAtIndexFunction, string>(new GetLockedTokenAtIndexFunction()
                { 
                    Index = y
                });
                lockedTokenRequest.Wait();

                var numLockTokenForTokenRequest = contractHandler.QueryAsync<GetNumLocksForTokenFunction, BigInteger>(new GetNumLocksForTokenFunction()
                {
                    LpToken = lockedTokenRequest.Result
                });
                numLockTokenForTokenRequest.Wait();
                for (var i = 0; i < (int)numLockTokenForTokenRequest.Result; i++)
                {
                    var tokenLockRequest = contractHandler.QueryDeserializingToObjectAsync<TokenLocksFunction, TokenLocksOutputDTO>(new TokenLocksFunction()
                    {
                        ReturnValue1 = lockedTokenRequest.Result,
                        ReturnValue2 = i
                    });
                    tokenLockRequest.Wait();
                    var lockedToken = tokenLockRequest.Result;
                    totalLiquidity += (double)Web3.Convert.FromWei(lockedToken.Amount);
                }
            }

            this.OutParameters["totalLockedLiquidity"].SetValue(totalLiquidity);
            return true;
        }
    }
}
