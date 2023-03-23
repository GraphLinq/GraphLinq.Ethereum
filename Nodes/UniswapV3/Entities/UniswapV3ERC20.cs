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
using static NodeBlock.Plugin.Ethereum.Nodes.UniswapV3.Entities.UniswapPairV3;

namespace NodeBlock.Plugin.Ethereum.Nodes.UniswapV3.Entities
{
    public class UniswapV3ERC20
    {
        public partial class UniswapV3ERC20Deployment : UniswapV3ERC20DeploymentBase
        {
            public UniswapV3ERC20Deployment() : base(BYTECODE) { }
            public UniswapV3ERC20Deployment(string byteCode) : base(byteCode) { }
        }

        public class UniswapV3ERC20DeploymentBase : ContractDeploymentMessage
        {
            public static string BYTECODE = "";
            public UniswapV3ERC20DeploymentBase() : base(BYTECODE) { }
            public UniswapV3ERC20DeploymentBase(string byteCode) : base(byteCode) { }

        }

        public partial class DOMAIN_SEPARATORFunctionV3 : DOMAIN_SEPARATORFunctionBaseV3 { }

        [Function("DOMAIN_SEPARATOR", "bytes32")]
        public class DOMAIN_SEPARATORFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class MINIMUM_LIQUIDITYFunctionV3 : MINIMUM_LIQUIDITYFunctionBaseV3 { }

        [Function("MINIMUM_LIQUIDITY", "uint256")]
        public class MINIMUM_LIQUIDITYFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class PERMIT_TYPEHASHFunctionV3 : PERMIT_TYPEHASHFunctionBaseV3 { }

        [Function("PERMIT_TYPEHASH", "bytes32")]
        public class PERMIT_TYPEHASHFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class AllowanceFunctionV3 : AllowanceFunctionBaseV3 { }

        [Function("allowance", "uint256")]
        public class AllowanceFunctionBaseV3 : FunctionMessage
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
            [Parameter("address", "", 2)]
            public virtual string ReturnValue2 { get; set; }
        }

        public partial class ApproveFunctionV3 : ApproveFunctionBaseV3 { }

        [Function("approve", "bool")]
        public class ApproveFunctionBaseV3 : FunctionMessage
        {
            [Parameter("address", "spender", 1)]
            public virtual string Spender { get; set; }
            [Parameter("uint256", "value", 2)]
            public virtual BigInteger Value { get; set; }
        }

        public partial class BalanceOfFunctionV3 : BalanceOfFunctionBaseV3 { }

        [Function("balanceOf", "uint256")]
        public class BalanceOfFunctionBaseV3 : FunctionMessage
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class BurnFunctionV3 : BurnFunctionBaseV3 { }

        [Function("burn", typeof(BurnOutputDTOV3))]
        public class BurnFunctionBaseV3 : FunctionMessage
        {
            [Parameter("address", "to", 1)]
            public virtual string To { get; set; }
        }

        public partial class DecimalsFunctionV3 : DecimalsFunctionBaseV3 { }

        [Function("decimals", "uint8")]
        public class DecimalsFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class FactoryFunctionV3 : FactoryFunctionBaseV3 { }

        [Function("factory", "address")]
        public class FactoryFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class GetReservesFunctionV3 : GetReservesFunctionBaseV3 { }

        [Function("getReserves", typeof(GetReservesOutputDTOV3))]
        public class GetReservesFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class InitializeFunctionV3 : InitializeFunctionBaseV3 { }

        [Function("initialize")]
        public class InitializeFunctionBaseV3 : FunctionMessage
        {
            [Parameter("address", "_token0", 1)]
            public virtual string Token0 { get; set; }
            [Parameter("address", "_token1", 2)]
            public virtual string Token1 { get; set; }
        }

        public partial class KLastFunctionV3 : KLastFunctionBaseV3 { }

        [Function("kLast", "uint256")]
        public class KLastFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class MintFunctionV3 : MintFunctionBaseV3 { }

        [Function("mint", "uint256")]
        public class MintFunctionBaseV3 : FunctionMessage
        {
            [Parameter("address", "to", 1)]
            public virtual string To { get; set; }
        }

        public partial class NameFunctionV3 : NameFunctionBaseV3 { }

        [Function("name", "string")]
        public class NameFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class NoncesFunctionV3 : NoncesFunctionBaseV3 { }

        [Function("nonces", "uint256")]
        public class NoncesFunctionBaseV3 : FunctionMessage
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class PermitFunctionV3 : PermitFunctionBaseV3 { }

        [Function("permit")]
        public class PermitFunctionBaseV3 : FunctionMessage
        {
            [Parameter("address", "owner", 1)]
            public virtual string Owner { get; set; }
            [Parameter("address", "spender", 2)]
            public virtual string Spender { get; set; }
            [Parameter("uint256", "value", 3)]
            public virtual BigInteger Value { get; set; }
            [Parameter("uint256", "deadline", 4)]
            public virtual BigInteger Deadline { get; set; }
            [Parameter("uint8", "v", 5)]
            public virtual byte V { get; set; }
            [Parameter("bytes32", "r", 6)]
            public virtual byte[] R { get; set; }
            [Parameter("bytes32", "s", 7)]
            public virtual byte[] S { get; set; }
        }

        public partial class Price0CumulativeLastFunctionV3 : Price0CumulativeLastFunctionBaseV3 { }

        [Function("price0CumulativeLast", "uint256")]
        public class Price0CumulativeLastFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class Price1CumulativeLastFunctionV3 : Price1CumulativeLastFunctionBaseV3 { }

        [Function("price1CumulativeLast", "uint256")]
        public class Price1CumulativeLastFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class SkimFunctionV3 : SkimFunctionBaseV3 { }

        [Function("skim")]
        public class SkimFunctionBaseV3 : FunctionMessage
        {
            [Parameter("address", "to", 1)]
            public virtual string To { get; set; }
        }

        public partial class SwapFunctionV3 : SwapFunctionBaseV3 { }

        [Function("swap")]
        public class SwapFunctionBaseV3 : FunctionMessage
        {
            [Parameter("uint256", "amount0Out", 1)]
            public virtual BigInteger Amount0Out { get; set; }
            [Parameter("uint256", "amount1Out", 2)]
            public virtual BigInteger Amount1Out { get; set; }
            [Parameter("address", "to", 3)]
            public virtual string To { get; set; }
            [Parameter("bytes", "data", 4)]
            public virtual byte[] Data { get; set; }
        }

        public partial class SymbolFunctionV3 : SymbolFunctionBaseV3 { }

        [Function("symbol", "string")]
        public class SymbolFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class SyncFunctionV3 : SyncFunctionBaseV3 { }

        [Function("sync")]
        public class SyncFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class Token0FunctionV3 : Token0FunctionBaseV3 { }

        [Function("token0", "address")]
        public class Token0FunctionBaseV3 : FunctionMessage
        {

        }

        public partial class Token1FunctionV3 : Token1FunctionBaseV3 { }

        [Function("token1", "address")]
        public class Token1FunctionBaseV3 : FunctionMessage
        {

        }

        public partial class TotalSupplyFunctionV3 : TotalSupplyFunctionBaseV3 { }

        [Function("totalSupply", "uint256")]
        public class TotalSupplyFunctionBaseV3 : FunctionMessage
        {

        }

        public partial class TransferFunctionV3 : TransferFunctionBaseV3 { }

        [Function("transfer", "bool")]
        public class TransferFunctionBaseV3 : FunctionMessage
        {
            [Parameter("address", "to", 1)]
            public virtual string To { get; set; }
            [Parameter("uint256", "value", 2)]
            public virtual BigInteger Value { get; set; }
        }

        public partial class TransferFromFunctionV3 : TransferFromFunctionBaseV3 { }

        [Function("transferFrom", "bool")]
        public class TransferFromFunctionBaseV3 : FunctionMessage
        {
            [Parameter("address", "from", 1)]
            public virtual string From { get; set; }
            [Parameter("address", "to", 2)]
            public virtual string To { get; set; }
            [Parameter("uint256", "value", 3)]
            public virtual BigInteger Value { get; set; }
        }

        public partial class ApprovalEventDTOV3 : ApprovalEventDTOBaseV3 { }

        [Event("Approval")]
        public class ApprovalEventDTOBaseV3 : IEventDTO
        {
            [Parameter("address", "owner", 1, true)]
            public virtual string Owner { get; set; }
            [Parameter("address", "spender", 2, true)]
            public virtual string Spender { get; set; }
            [Parameter("uint256", "value", 3, false)]
            public virtual BigInteger Value { get; set; }
        }

        public partial class BurnEventDTOV3 : BurnEventDTOBaseV3 { }

        [Event("Burn")]
        public class BurnEventDTOBaseV3 : IEventDTO
        {
            [Parameter("address", "sender", 1, true)]
            public virtual string Sender { get; set; }
            [Parameter("uint256", "amount0", 2, false)]
            public virtual BigInteger Amount0 { get; set; }
            [Parameter("uint256", "amount1", 3, false)]
            public virtual BigInteger Amount1 { get; set; }
            [Parameter("address", "to", 4, true)]
            public virtual string To { get; set; }
        }

        public partial class MintEventDTOV3 : MintEventDTOBaseV3 { }

        [Event("Mint")]
        public class MintEventDTOBaseV3 : IEventDTO
        {
            [Parameter("address", "sender", 1, true)]
            public virtual string Sender { get; set; }
            [Parameter("uint256", "amount0", 2, false)]
            public virtual BigInteger Amount0 { get; set; }
            [Parameter("uint256", "amount1", 3, false)]
            public virtual BigInteger Amount1 { get; set; }
        }

        public partial class SwapEventDTOV3 : SwapEventDTOBaseV3 { }

        [Event("Swap")]
        public class SwapEventDTOBaseV3 : IEventDTO
        {
            [Parameter("address", "sender", 1, true)]
            public virtual string Sender { get; set; }
            [Parameter("uint256", "amount0In", 2, false)]
            public virtual BigInteger Amount0In { get; set; }
            [Parameter("uint256", "amount1In", 3, false)]
            public virtual BigInteger Amount1In { get; set; }
            [Parameter("uint256", "amount0Out", 4, false)]
            public virtual BigInteger Amount0Out { get; set; }
            [Parameter("uint256", "amount1Out", 5, false)]
            public virtual BigInteger Amount1Out { get; set; }
            [Parameter("address", "to", 6, true)]
            public virtual string To { get; set; }
        }

        public partial class SyncEventDTOV3 : SyncEventDTOBaseV3 { }

        [Event("Sync")]
        public class SyncEventDTOBaseV3 : IEventDTO
        {
            [Parameter("uint112", "reserve0", 1, false)]
            public virtual BigInteger Reserve0 { get; set; }
            [Parameter("uint112", "reserve1", 2, false)]
            public virtual BigInteger Reserve1 { get; set; }
        }

        public partial class TransferEventDTOV3 : TransferEventDTOBaseV3 { }

        [Event("Transfer")]
        public class TransferEventDTOBaseV3 : IEventDTO
        {
            [Parameter("address", "from", 1, true)]
            public virtual string From { get; set; }
            [Parameter("address", "to", 2, true)]
            public virtual string To { get; set; }
            [Parameter("uint256", "value", 3, false)]
            public virtual BigInteger Value { get; set; }
        }

        public partial class DOMAIN_SEPARATOROutputDTOV3 : DOMAIN_SEPARATOROutputDTOBaseV3 { }

        [FunctionOutput]
        public class DOMAIN_SEPARATOROutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("bytes32", "", 1)]
            public virtual byte[] ReturnValue1 { get; set; }
        }

        public partial class MINIMUM_LIQUIDITYOutputDTOV3 : MINIMUM_LIQUIDITYOutputDTOBaseV3 { }

        [FunctionOutput]
        public class MINIMUM_LIQUIDITYOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }

        public partial class PERMIT_TYPEHASHOutputDTOV3 : PERMIT_TYPEHASHOutputDTOBaseV3 { }

        [FunctionOutput]
        public class PERMIT_TYPEHASHOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("bytes32", "", 1)]
            public virtual byte[] ReturnValue1 { get; set; }
        }

        public partial class AllowanceOutputDTOV3 : AllowanceOutputDTOBaseV3 { }

        [FunctionOutput]
        public class AllowanceOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }



        public partial class BalanceOfOutputDTOV3 : BalanceOfOutputDTOBaseV3 { }

        [FunctionOutput]
        public class BalanceOfOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }

        public partial class BurnOutputDTOV3 : BurnOutputDTOBaseV3 { }

        [FunctionOutput]
        public class BurnOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("uint256", "amount0", 1)]
            public virtual BigInteger Amount0 { get; set; }
            [Parameter("uint256", "amount1", 2)]
            public virtual BigInteger Amount1 { get; set; }
        }

        public partial class DecimalsOutputDTOV3 : DecimalsOutputDTOBaseV3 { }

        [FunctionOutput]
        public class DecimalsOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("uint8", "", 1)]
            public virtual byte ReturnValue1 { get; set; }
        }

        public partial class FactoryOutputDTOV3 : FactoryOutputDTOBaseV3 { }

        [FunctionOutput]
        public class FactoryOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class GetReservesOutputDTOV3 : GetReservesOutputDTOBaseV3 { }

        [FunctionOutput]
        public class GetReservesOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("uint112", "_reserve0", 1)]
            public virtual BigInteger Reserve0 { get; set; }
            [Parameter("uint112", "_reserve1", 2)]
            public virtual BigInteger Reserve1 { get; set; }
            [Parameter("uint32", "_blockTimestampLast", 3)]
            public virtual uint BlockTimestampLast { get; set; }
        }



        public partial class KLastOutputDTOV3 : KLastOutputDTOBaseV3 { }

        [FunctionOutput]
        public class KLastOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }



        public partial class NameOutputDTOV3 : NameOutputDTOBaseV3 { }

        [FunctionOutput]
        public class NameOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("string", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class NoncesOutputDTOV3 : NoncesOutputDTOBaseV3 { }

        [FunctionOutput]
        public class NoncesOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }



        public partial class Price0CumulativeLastOutputDTOV3 : Price0CumulativeLastOutputDTOBaseV3 { }

        [FunctionOutput]
        public class Price0CumulativeLastOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }

        public partial class Price1CumulativeLastOutputDTOV3 : Price1CumulativeLastOutputDTOBaseV3 { }

        [FunctionOutput]
        public class Price1CumulativeLastOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }


        public partial class SymbolOutputDTOV3 : SymbolOutputDTOBaseV3 { }

        [FunctionOutput]
        public class SymbolOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("string", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }



        public partial class Token0OutputDTOV3 : Token0OutputDTOBaseV3 { }

        [FunctionOutput]
        public class Token0OutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class Token1OutputDTOV3 : Token1OutputDTOBaseV3 { }

        [FunctionOutput]
        public class Token1OutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class TotalSupplyOutputDTOV3 : TotalSupplyOutputDTOBaseV3 { }

        [FunctionOutput]
        public class TotalSupplyOutputDTOBaseV3 : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }


    }
}
