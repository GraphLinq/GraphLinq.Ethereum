using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using static NodeBlock.Plugin.Ethereum.Nodes.Quickswap.Entities.QuickswapFactoryV2;
using static NodeBlock.Plugin.Ethereum.Nodes.Quickswap.Entities.QuickswapPair;
using static NodeBlock.Plugin.Ethereum.Nodes.Quickswap.Entities.QuickswapRouterV2;

namespace NodeBlock.Plugin.Ethereum.Nodes.Quickswap.Swap
{
    [NodeDefinition("RequestQuickswapSwapNode", "Swap ETH For Exact Tokens", NodeTypeEnum.Function, "Quickswap")]
    [NodeGraphDescription("Request a swap on Quickswap for a pair")]
    [NodeSpecialActionAttribute("How to use the Quickswap Swap Block ?", "open_url", "#")]
    [NodeIDEParameters(Hidden = true)]
    public class RequestQuickswapSwapNode : Node
    {
        public RequestQuickswapSwapNode(string id, BlockGraph graph)
            : base(id, graph, typeof(RequestQuickswapSwapNode).Name)
        {
            this.InParameters.Add("ethConnection", new NodeParameter(this, "ethConnection", typeof(object), true));
            this.InParameters.Add("managedWallet", new NodeParameter(this, "managedWallet", typeof(Wallet.ManagedWallet.ManagedWallet), true));
            this.InParameters.Add("tokenAddress", new NodeParameter(this, "tokenAddress", typeof(string), true));
            this.InParameters.Add("amountOut", new NodeParameter(this, "amountOut", typeof(double), true));
            this.InParameters.Add("slippage", new NodeParameter(this, "slippage", typeof(double), true));
        }

        public const string ROUTER_ADDR = "0x7a250d5630b4cf539739df2c5dacb4c659f2488d";
        public const string FACTORY_ADDR = "0x5C69bEe701ef814a2B6a3EDD4B1652CB9cc5aA6f";

        public override bool CanBeExecuted => true;

        public override bool CanExecute => true;

        public override bool OnExecution()
        {
            asyncWrapper().Wait();
            return true;
        }

        private async Task asyncWrapper()
        {
            EthConnection ethConnection = this.InParameters["ethConnection"].GetValue() as EthConnection;

            // Instanciate web3 account
            var managedWallet = this.InParameters["managedWallet"].GetValue() as Wallet.ManagedWallet.ManagedWallet;
            var web3Account = ethConnection.InstanciateWeb3Account(managedWallet.GetAccount());

            var tokenAddr = this.InParameters["tokenAddress"].GetValue().ToString();
            var amountOut = double.Parse(this.InParameters["amountOut"].GetValue().ToString(), CultureInfo.InvariantCulture);
            var slippage = double.Parse(this.InParameters["slippage"].GetValue().ToString(), CultureInfo.InvariantCulture);
            var contractHandler = web3Account.Eth.GetContractHandler(ROUTER_ADDR);
            var contractFactoryHandler = web3Account.Eth.GetContractHandler(FACTORY_ADDR);
            var wethAddr = await contractHandler.QueryAsync<WETHFunction, string>(new WETHFunction());
            var latestBlockNumber = await web3Account.Eth.Blocks.GetBlockNumber.SendRequestAsync();
            var beforeBlockNumberBlockNumber = await web3Account.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(new HexBigInteger(latestBlockNumber.Value - 1));
            var latestBlock = await web3Account.Eth.Blocks.GetBlockWithTransactionsHashesByNumber.SendRequestAsync(latestBlockNumber);

            // Try to estimate the eth price for the swap
            var pairAddr = await contractFactoryHandler.QueryAsync<GetPairFunction, string>(new GetPairFunction()
            {
                ReturnValue1 = wethAddr,
                ReturnValue2 = tokenAddr
            });
            var pairContractHandler = web3Account.Eth.GetContractHandler(pairAddr);
            var reserves = await pairContractHandler.QueryDeserializingToObjectAsync<GetReservesFunction, GetReservesOutputDTO>(new GetReservesFunction());

            var quote = await contractHandler.QueryAsync<QuoteFunction, BigInteger>(new QuoteFunction()
            {
                AmountA = Web3.Convert.ToWei(1),
                ReserveA = reserves.Reserve0,
                ReserveB = reserves.Reserve1
            });
            var ethEstimated = (double)Web3.Convert.FromWei(quote) * amountOut;
            ethEstimated = ethEstimated + (ethEstimated / 100 * slippage);

            // Send tx
            await contractHandler.SendRequestAsync<SwapETHForExactTokensFunctionBase>(new SwapETHForExactTokensFunctionBase()
            {
                AmountOut = Web3.Convert.ToWei(amountOut),
                Path = new List<string>() { wethAddr, tokenAddr },
                To = managedWallet.ManagedWalletEntity.PublicKey,
                Deadline = latestBlock.Timestamp.Value + ((latestBlock.Timestamp.Value - beforeBlockNumberBlockNumber.Timestamp.Value) * 10),
                AmountToSend = Web3.Convert.ToWei(ethEstimated),
                GasPrice = Web3.Convert.ToWei(managedWallet.Gwei, UnitConversion.EthUnit.Gwei),
            });
        }
    }
}
