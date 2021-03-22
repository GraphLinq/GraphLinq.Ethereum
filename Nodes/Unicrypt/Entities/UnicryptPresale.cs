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

namespace NodeBlock.Plugin.Ethereum.Nodes.Unicrypt.Entities
{
    public class UnicryptPresale
    {
        public partial class UnicryptPresaleDeployment : UnicryptPresaleDeploymentBase
        {
            public UnicryptPresaleDeployment() : base(BYTECODE) { }
            public UnicryptPresaleDeployment(string byteCode) : base(byteCode) { }
        }

        public class UnicryptPresaleDeploymentBase : ContractDeploymentMessage
        {
            public static string BYTECODE = "";
            public UnicryptPresaleDeploymentBase() : base(BYTECODE) { }
            public UnicryptPresaleDeploymentBase(string byteCode) : base(byteCode) { }
            [Parameter("address", "_presaleGenerator", 1)]
            public virtual string PresaleGenerator { get; set; }
        }

        public partial class BUYERSFunction : BUYERSFunctionBase { }

        [Function("BUYERS", typeof(BUYERSOutputDTO))]
        public class BUYERSFunctionBase : FunctionMessage
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class CONTRACT_VERSIONFunction : CONTRACT_VERSIONFunctionBase { }

        [Function("CONTRACT_VERSION", "uint256")]
        public class CONTRACT_VERSIONFunctionBase : FunctionMessage
        {

        }

        public partial class PRESALE_FEE_INFOFunction : PRESALE_FEE_INFOFunctionBase { }

        [Function("PRESALE_FEE_INFO", typeof(PRESALE_FEE_INFOOutputDTO))]
        public class PRESALE_FEE_INFOFunctionBase : FunctionMessage
        {

        }

        public partial class PRESALE_GENERATORFunction : PRESALE_GENERATORFunctionBase { }

        [Function("PRESALE_GENERATOR", "address")]
        public class PRESALE_GENERATORFunctionBase : FunctionMessage
        {

        }

        public partial class PRESALE_INFOFunction : PRESALE_INFOFunctionBase { }

        [Function("PRESALE_INFO", typeof(PRESALE_INFOOutputDTO))]
        public class PRESALE_INFOFunctionBase : FunctionMessage
        {

        }

        public partial class PRESALE_LOCK_FORWARDERFunction : PRESALE_LOCK_FORWARDERFunctionBase { }

        [Function("PRESALE_LOCK_FORWARDER", "address")]
        public class PRESALE_LOCK_FORWARDERFunctionBase : FunctionMessage
        {

        }

        public partial class PRESALE_SETTINGSFunction : PRESALE_SETTINGSFunctionBase { }

        [Function("PRESALE_SETTINGS", "address")]
        public class PRESALE_SETTINGSFunctionBase : FunctionMessage
        {

        }

        public partial class STATUSFunction : STATUSFunctionBase { }

        [Function("STATUS", typeof(STATUSOutputDTO))]
        public class STATUSFunctionBase : FunctionMessage
        {

        }

        public partial class UNI_FACTORYFunction : UNI_FACTORYFunctionBase { }

        [Function("UNI_FACTORY", "address")]
        public class UNI_FACTORYFunctionBase : FunctionMessage
        {

        }

        public partial class WETHFunction : WETHFunctionBase { }

        [Function("WETH", "address")]
        public class WETHFunctionBase : FunctionMessage
        {

        }

        public partial class AddLiquidityFunction : AddLiquidityFunctionBase { }

        [Function("addLiquidity")]
        public class AddLiquidityFunctionBase : FunctionMessage
        {

        }

        public partial class EditWhitelistFunction : EditWhitelistFunctionBase { }

        [Function("editWhitelist")]
        public class EditWhitelistFunctionBase : FunctionMessage
        {
            [Parameter("address[]", "_users", 1)]
            public virtual List<string> Users { get; set; }
            [Parameter("bool", "_add", 2)]
            public virtual bool Add { get; set; }
        }

        public partial class ForceFailByUnicryptFunction : ForceFailByUnicryptFunctionBase { }

        [Function("forceFailByUnicrypt")]
        public class ForceFailByUnicryptFunctionBase : FunctionMessage
        {

        }

        public partial class ForceFailIfPairExistsFunction : ForceFailIfPairExistsFunctionBase { }

        [Function("forceFailIfPairExists")]
        public class ForceFailIfPairExistsFunctionBase : FunctionMessage
        {

        }

        public partial class GetUserWhitelistStatusFunction : GetUserWhitelistStatusFunctionBase { }

        [Function("getUserWhitelistStatus", "bool")]
        public class GetUserWhitelistStatusFunctionBase : FunctionMessage
        {
            [Parameter("address", "_user", 1)]
            public virtual string User { get; set; }
        }

        public partial class GetWhitelistedUserAtIndexFunction : GetWhitelistedUserAtIndexFunctionBase { }

        [Function("getWhitelistedUserAtIndex", "address")]
        public class GetWhitelistedUserAtIndexFunctionBase : FunctionMessage
        {
            [Parameter("uint256", "_index", 1)]
            public virtual BigInteger Index { get; set; }
        }

        public partial class GetWhitelistedUsersLengthFunction : GetWhitelistedUsersLengthFunctionBase { }

        [Function("getWhitelistedUsersLength", "uint256")]
        public class GetWhitelistedUsersLengthFunctionBase : FunctionMessage
        {

        }

        public partial class Init1Function : Init1FunctionBase { }

        [Function("init1")]
        public class Init1FunctionBase : FunctionMessage
        {
            [Parameter("address", "_presaleOwner", 1)]
            public virtual string PresaleOwner { get; set; }
            [Parameter("uint256", "_amount", 2)]
            public virtual BigInteger Amount { get; set; }
            [Parameter("uint256", "_tokenPrice", 3)]
            public virtual BigInteger TokenPrice { get; set; }
            [Parameter("uint256", "_maxEthPerBuyer", 4)]
            public virtual BigInteger MaxEthPerBuyer { get; set; }
            [Parameter("uint256", "_hardcap", 5)]
            public virtual BigInteger Hardcap { get; set; }
            [Parameter("uint256", "_softcap", 6)]
            public virtual BigInteger Softcap { get; set; }
            [Parameter("uint256", "_liquidityPercent", 7)]
            public virtual BigInteger LiquidityPercent { get; set; }
            [Parameter("uint256", "_listingRate", 8)]
            public virtual BigInteger ListingRate { get; set; }
            [Parameter("uint256", "_startblock", 9)]
            public virtual BigInteger Startblock { get; set; }
            [Parameter("uint256", "_endblock", 10)]
            public virtual BigInteger Endblock { get; set; }
            [Parameter("uint256", "_lockPeriod", 11)]
            public virtual BigInteger LockPeriod { get; set; }
        }

        public partial class Init2Function : Init2FunctionBase { }

        [Function("init2")]
        public class Init2FunctionBase : FunctionMessage
        {
            [Parameter("address", "_baseToken", 1)]
            public virtual string BaseToken { get; set; }
            [Parameter("address", "_presaleToken", 2)]
            public virtual string PresaleToken { get; set; }
            [Parameter("uint256", "_unicryptBaseFee", 3)]
            public virtual BigInteger UnicryptBaseFee { get; set; }
            [Parameter("uint256", "_unicryptTokenFee", 4)]
            public virtual BigInteger UnicryptTokenFee { get; set; }
            [Parameter("uint256", "_referralFee", 5)]
            public virtual BigInteger ReferralFee { get; set; }
            [Parameter("address", "_baseFeeAddress", 6)]
            public virtual string BaseFeeAddress { get; set; }
            [Parameter("address", "_tokenFeeAddress", 7)]
            public virtual string TokenFeeAddress { get; set; }
            [Parameter("address", "_referralAddress", 8)]
            public virtual string ReferralAddress { get; set; }
        }

        public partial class OwnerWithdrawTokensFunction : OwnerWithdrawTokensFunctionBase { }

        [Function("ownerWithdrawTokens")]
        public class OwnerWithdrawTokensFunctionBase : FunctionMessage
        {

        }

        public partial class PresaleStatusFunction : PresaleStatusFunctionBase { }

        [Function("presaleStatus", "uint256")]
        public class PresaleStatusFunctionBase : FunctionMessage
        {

        }

        public partial class SetWhitelistFlagFunction : SetWhitelistFlagFunctionBase { }

        [Function("setWhitelistFlag")]
        public class SetWhitelistFlagFunctionBase : FunctionMessage
        {
            [Parameter("bool", "_flag", 1)]
            public virtual bool Flag { get; set; }
        }

        public partial class UpdateBlocksFunction : UpdateBlocksFunctionBase { }

        [Function("updateBlocks")]
        public class UpdateBlocksFunctionBase : FunctionMessage
        {
            [Parameter("uint256", "_startBlock", 1)]
            public virtual BigInteger StartBlock { get; set; }
            [Parameter("uint256", "_endBlock", 2)]
            public virtual BigInteger EndBlock { get; set; }
        }

        public partial class UpdateMaxSpendLimitFunction : UpdateMaxSpendLimitFunctionBase { }

        [Function("updateMaxSpendLimit")]
        public class UpdateMaxSpendLimitFunctionBase : FunctionMessage
        {
            [Parameter("uint256", "_maxSpend", 1)]
            public virtual BigInteger MaxSpend { get; set; }
        }

        public partial class UserDepositFunction : UserDepositFunctionBase { }

        [Function("userDeposit")]
        public class UserDepositFunctionBase : FunctionMessage
        {
            [Parameter("uint256", "_amount", 1)]
            public virtual BigInteger Amount { get; set; }
        }

        public partial class UserWithdrawBaseTokensFunction : UserWithdrawBaseTokensFunctionBase { }

        [Function("userWithdrawBaseTokens")]
        public class UserWithdrawBaseTokensFunctionBase : FunctionMessage
        {

        }

        public partial class UserWithdrawTokensFunction : UserWithdrawTokensFunctionBase { }

        [Function("userWithdrawTokens")]
        public class UserWithdrawTokensFunctionBase : FunctionMessage
        {

        }

        public partial class BUYERSOutputDTO : BUYERSOutputDTOBase { }

        [FunctionOutput]
        public class BUYERSOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("uint256", "baseDeposited", 1)]
            public virtual BigInteger BaseDeposited { get; set; }
            [Parameter("uint256", "tokensOwed", 2)]
            public virtual BigInteger TokensOwed { get; set; }
        }

        public partial class CONTRACT_VERSIONOutputDTO : CONTRACT_VERSIONOutputDTOBase { }

        [FunctionOutput]
        public class CONTRACT_VERSIONOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }

        public partial class PRESALE_FEE_INFOOutputDTO : PRESALE_FEE_INFOOutputDTOBase { }

        [FunctionOutput]
        public class PRESALE_FEE_INFOOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("uint256", "UNICRYPT_BASE_FEE", 1)]
            public virtual BigInteger UNICRYPT_BASE_FEE { get; set; }
            [Parameter("uint256", "UNICRYPT_TOKEN_FEE", 2)]
            public virtual BigInteger UNICRYPT_TOKEN_FEE { get; set; }
            [Parameter("uint256", "REFERRAL_FEE", 3)]
            public virtual BigInteger REFERRAL_FEE { get; set; }
            [Parameter("address", "BASE_FEE_ADDRESS", 4)]
            public virtual string BASE_FEE_ADDRESS { get; set; }
            [Parameter("address", "TOKEN_FEE_ADDRESS", 5)]
            public virtual string TOKEN_FEE_ADDRESS { get; set; }
            [Parameter("address", "REFERRAL_FEE_ADDRESS", 6)]
            public virtual string REFERRAL_FEE_ADDRESS { get; set; }
        }

        public partial class PRESALE_GENERATOROutputDTO : PRESALE_GENERATOROutputDTOBase { }

        [FunctionOutput]
        public class PRESALE_GENERATOROutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class PRESALE_INFOOutputDTO : PRESALE_INFOOutputDTOBase { }

        [FunctionOutput]
        public class PRESALE_INFOOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("address", "PRESALE_OWNER", 1)]
            public virtual string PRESALE_OWNER { get; set; }
            [Parameter("address", "S_TOKEN", 2)]
            public virtual string S_TOKEN { get; set; }
            [Parameter("address", "B_TOKEN", 3)]
            public virtual string B_TOKEN { get; set; }
            [Parameter("uint256", "TOKEN_PRICE", 4)]
            public virtual BigInteger TOKEN_PRICE { get; set; }
            [Parameter("uint256", "MAX_SPEND_PER_BUYER", 5)]
            public virtual BigInteger MAX_SPEND_PER_BUYER { get; set; }
            [Parameter("uint256", "AMOUNT", 6)]
            public virtual BigInteger AMOUNT { get; set; }
            [Parameter("uint256", "HARDCAP", 7)]
            public virtual BigInteger HARDCAP { get; set; }
            [Parameter("uint256", "SOFTCAP", 8)]
            public virtual BigInteger SOFTCAP { get; set; }
            [Parameter("uint256", "LIQUIDITY_PERCENT", 9)]
            public virtual BigInteger LIQUIDITY_PERCENT { get; set; }
            [Parameter("uint256", "LISTING_RATE", 10)]
            public virtual BigInteger LISTING_RATE { get; set; }
            [Parameter("uint256", "START_BLOCK", 11)]
            public virtual BigInteger START_BLOCK { get; set; }
            [Parameter("uint256", "END_BLOCK", 12)]
            public virtual BigInteger END_BLOCK { get; set; }
            [Parameter("uint256", "LOCK_PERIOD", 13)]
            public virtual BigInteger LOCK_PERIOD { get; set; }
            [Parameter("bool", "PRESALE_IN_ETH", 14)]
            public virtual bool PRESALE_IN_ETH { get; set; }
        }

        public partial class PRESALE_LOCK_FORWARDEROutputDTO : PRESALE_LOCK_FORWARDEROutputDTOBase { }

        [FunctionOutput]
        public class PRESALE_LOCK_FORWARDEROutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class PRESALE_SETTINGSOutputDTO : PRESALE_SETTINGSOutputDTOBase { }

        [FunctionOutput]
        public class PRESALE_SETTINGSOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class STATUSOutputDTO : STATUSOutputDTOBase { }

        [FunctionOutput]
        public class STATUSOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("bool", "WHITELIST_ONLY", 1)]
            public virtual bool WHITELIST_ONLY { get; set; }
            [Parameter("bool", "LP_GENERATION_COMPLETE", 2)]
            public virtual bool LP_GENERATION_COMPLETE { get; set; }
            [Parameter("bool", "FORCE_FAILED", 3)]
            public virtual bool FORCE_FAILED { get; set; }
            [Parameter("uint256", "TOTAL_BASE_COLLECTED", 4)]
            public virtual BigInteger TOTAL_BASE_COLLECTED { get; set; }
            [Parameter("uint256", "TOTAL_TOKENS_SOLD", 5)]
            public virtual BigInteger TOTAL_TOKENS_SOLD { get; set; }
            [Parameter("uint256", "TOTAL_TOKENS_WITHDRAWN", 6)]
            public virtual BigInteger TOTAL_TOKENS_WITHDRAWN { get; set; }
            [Parameter("uint256", "TOTAL_BASE_WITHDRAWN", 7)]
            public virtual BigInteger TOTAL_BASE_WITHDRAWN { get; set; }
            [Parameter("uint256", "ROUND1_LENGTH", 8)]
            public virtual BigInteger ROUND1_LENGTH { get; set; }
            [Parameter("uint256", "NUM_BUYERS", 9)]
            public virtual BigInteger NUM_BUYERS { get; set; }
        }

        public partial class UNI_FACTORYOutputDTO : UNI_FACTORYOutputDTOBase { }

        [FunctionOutput]
        public class UNI_FACTORYOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class WETHOutputDTO : WETHOutputDTOBase { }

        [FunctionOutput]
        public class WETHOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }









        public partial class GetUserWhitelistStatusOutputDTO : GetUserWhitelistStatusOutputDTOBase { }

        [FunctionOutput]
        public class GetUserWhitelistStatusOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("bool", "", 1)]
            public virtual bool ReturnValue1 { get; set; }
        }

        public partial class GetWhitelistedUserAtIndexOutputDTO : GetWhitelistedUserAtIndexOutputDTOBase { }

        [FunctionOutput]
        public class GetWhitelistedUserAtIndexOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("address", "", 1)]
            public virtual string ReturnValue1 { get; set; }
        }

        public partial class GetWhitelistedUsersLengthOutputDTO : GetWhitelistedUsersLengthOutputDTOBase { }

        [FunctionOutput]
        public class GetWhitelistedUsersLengthOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }







        public partial class PresaleStatusOutputDTO : PresaleStatusOutputDTOBase { }

        [FunctionOutput]
        public class PresaleStatusOutputDTOBase : IFunctionOutputDTO
        {
            [Parameter("uint256", "", 1)]
            public virtual BigInteger ReturnValue1 { get; set; }
        }

    }
}
