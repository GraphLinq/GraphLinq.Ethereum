using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using static NodeBlock.Plugin.Ethereum.Nodes.Unicrypt.Entities.UniswapV2Locker;

namespace NodeBlock.Plugin.Ethereum.Nodes.Unicrypt
{
    [NodeDefinition("GetLockedTokenParametersNode", "Get Unicrypt Locked Token Parameters", NodeTypeEnum.Function, "Unicrypt")]
    [NodeGraphDescription("Get parameters for locked token")]
    public class GetLockedTokenParametersNode : Node
    {
        public GetLockedTokenParametersNode(string id, BlockGraph graph)
            : base(id, graph, typeof(GetLockedTokenParametersNode).Name)
        {
            this.InParameters.Add("lockedTokenObject", new NodeParameter(this, "lockedTokenObject", typeof(object), true));

            this.OutParameters.Add("lockDate", new NodeParameter(this, "lockDate", typeof(long), false));
            this.OutParameters.Add("amount", new NodeParameter(this, "amount", typeof(double), false));
            this.OutParameters.Add("initialAmount", new NodeParameter(this, "initialAmount", typeof(double), false));
            this.OutParameters.Add("unlockDate", new NodeParameter(this, "unlockDate", typeof(long), false));
            this.OutParameters.Add("lockID", new NodeParameter(this, "lockID", typeof(int), false));
            this.OutParameters.Add("owner", new NodeParameter(this, "owner", typeof(string), false));
        }

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            var lockedTokenObject = this.InParameters["lockedTokenObject"].GetValue() as TokenLocksOutputDTO;

            this.OutParameters["lockDate"].SetValue((long)lockedTokenObject.LockDate);
            this.OutParameters["amount"].SetValue((double)Web3.Convert.FromWei(lockedTokenObject.Amount));
            this.OutParameters["initialAmount"].SetValue((double)Web3.Convert.FromWei(lockedTokenObject.InitialAmount));
            this.OutParameters["unlockDate"].SetValue((long)lockedTokenObject.UnlockDate);
            this.OutParameters["lockID"].SetValue((int)lockedTokenObject.LockID);
            this.OutParameters["owner"].SetValue(lockedTokenObject.Owner);

            return true;
        }
    }
}
