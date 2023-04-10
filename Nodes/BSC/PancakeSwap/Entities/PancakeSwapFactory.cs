using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Numerics;
using Nethereum.Hex.HexTypes;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Web3;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.Contracts.CQS;
using Nethereum.Contracts;
using System.Threading;

namespace NodeBlock.Plugin.Ethereum.Nodes.BSC.PankakeSwap.Entities
{
    public class PancakeSwapFactory
    {
        public partial class PancakeSwapFactoryDeployment : PancakeSwapFactoryDeploymentBase
        {
            public PancakeSwapFactoryDeployment() : base(BYTECODE) { }
            public PancakeSwapFactoryDeployment(string byteCode) : base(byteCode) { }
        }

        public class PancakeSwapFactoryDeploymentBase : ContractDeploymentMessage
        {
            public static string BYTECODE = "";
            public PancakeSwapFactoryDeploymentBase() : base(BYTECODE) { }
            public PancakeSwapFactoryDeploymentBase(string byteCode) : base(byteCode) { }
            [Parameter("address", "_feeToSetter", 1)]
            public virtual string FeeToSetter { get; set; }
        }

        public partial class INIT_CODE_PAIR_HASHFunction : INIT_CODE_PAIR_HASHFunctionBase { }

        [Function("INIT_CODE_PAIR_HASH", "bytes32")]
        public class INIT_CODE_PAIR_HASHFunctionBase : FunctionMessage
        {

        }

        public partial class AllPairsFunction : AllPairsFunctionBase { }

        [Function("allPairs", "address")]
        public class AllPairsFunctionBase : FunctionMessage
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }

        public partial class AllPairsLengthFunction : AllPairsLengthFunctionBase { }

        [Function("allPairsLength", "uint256")]
        public class AllPairsLengthFunctionBase : FunctionMessage
        {

        }

        public partial class CreatePairFunction : CreatePairFunctionBase { }

        [Function("createPair", "address")]
        public class CreatePairFunctionBase : FunctionMessage
        {
            [Parameter("address", "tokenA", 1)]
            public virtual string TokenA { get; set; }
            [Parameter("address", "tokenB", 2)]
            public virtual string TokenB { get; set; }
        }

        public partial class FeeToFunction : FeeToFunctionBase { }

        [Function("feeTo", "address")]
        public class FeeToFunctionBase : FunctionMessage
        {

        }

        public partial class FeeToSetterFunction : FeeToSetterFunctionBase { }

        [Function("feeToSetter", "address")]
        public class FeeToSetterFunctionBase : FunctionMessage
        {

        }

        public partial class GetPairFunction : GetPairFunctionBase { }

        [Function("getPair", "address")]
        public class GetPairFunctionBase : FunctionMessage
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
            [Parameter("address", "", 2)]
            public virtual string ReturnValue2 { get; set; }
        }

        public partial class SetFeeToFunction : SetFeeToFunctionBase { }

        [Function("setFeeTo")]
        public class SetFeeToFunctionBase : FunctionMessage
        {
            [Parameter("address", "_feeTo", 1)]
            public virtual string FeeTo { get; set; }
        }

        public partial class SetFeeToSetterFunction : SetFeeToSetterFunctionBase { }

        [Function("setFeeToSetter")]
        public class SetFeeToSetterFunctionBase : FunctionMessage
        {
            [Parameter("address", "_feeToSetter", 1)]
            public virtual string FeeToSetter { get; set; }
        }

        public partial class PairCreatedEventDTO : PairCreatedEventDTOBase { }

        [Event("PairCreated")]
        public class PairCreatedEventDTOBase : IEventDTO
        {
            [Parameter("address", "token0", 1, true)]
            public virtual string Token0 { get; set; }
            [Parameter("address", "token1", 2, true)]
            public virtual string Token1 { get; set; }
            [Parameter("address", "pair", 3, false)]
            public virtual string Pair { get; set; }
            [Parameter("uint256", "", 4, false)]
            public virtual BigInteger ReturnValue4 { get; set; }
        }

        public partial class INIT_CODE_PAIR_HASHOutputDTO : INIT_CODE_PAIR_HASHOutputDTOBase { }

        [FunctionOutput]
        public class INIT_CODE_PAIR_HASHOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("bytes32", "", 1)]
            public virtual byte[] ReturnValue1 { get; set; }
        }

        public partial class AllPairsOutputDTO : AllPairsOutputDTOBase { }

        [FunctionOutput]
        public class AllPairsOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class AllPairsLengthOutputDTO : AllPairsLengthOutputDTOBase { }

        [FunctionOutput]
        public class AllPairsLengthOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }



        public partial class FeeToOutputDTO : FeeToOutputDTOBase { }

        [FunctionOutput]
        public class FeeToOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class FeeToSetterOutputDTO : FeeToSetterOutputDTOBase { }

        [FunctionOutput]
        public class FeeToSetterOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class GetPairOutputDTO : GetPairOutputDTOBase { }

        [FunctionOutput]
        public class GetPairOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

    }
}
