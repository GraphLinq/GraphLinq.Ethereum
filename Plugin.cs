using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.Web3;
using NodeBlock.Engine;
using NodeBlock.Engine.Interop;
using NodeBlock.Engine.Interop.Plugin;
using System;
using System.Collections.Generic;
using System.Text;
using Nethereum.JsonRpc.Client.Streaming;
using Nethereum.RPC.Eth.Subscriptions;
using Nethereum.RPC.Eth.DTOs;
using System.Collections.Concurrent;
using System.Linq;
using NodeBlock.Plugin.Ethereum.Nodes;
using System.Threading.Tasks;

namespace NodeBlock.Plugin.Ethereum
{
    public class Plugin : BasePlugin
    {
        public static string WEB_3_API_URL_ETH = "";
        public static string WEB_3_WS_URL_ETH = "";

        public static string WEB_3_API_URL_BSC = "";
        public static string WEB_3_WS_URL_BSC = "";

        public static object mutex = new object();

        public static Web3 Web3ClientETH { get; set; }
        public static StreamingWebSocketClient SocketClientETH { get; set; }

        public static Web3 Web3ClientBSC { get; set; }
        public static StreamingWebSocketClient SocketClientBSC { get; set; }

        public static ManagedEthereumEvents EventsManagerEth { get; set; }

        public static ManagedEthereumEvents EventsManagerBsc { get; set; }

        public static bool PluginAlive = true;

        public override void Load()
        {
            // ETH
            WEB_3_API_URL_ETH = Environment.GetEnvironmentVariable("eth_api_http_url");
            WEB_3_WS_URL_ETH = Environment.GetEnvironmentVariable("eth_api_ws_url");

            Web3ClientETH = new Web3(WEB_3_API_URL_ETH);
            SocketClientETH = new StreamingWebSocketClient(WEB_3_WS_URL_ETH);

            SocketClientETH.StartAsync().Wait();


            // BSC
            WEB_3_API_URL_BSC = Environment.GetEnvironmentVariable("bsc_api_http_url");
            WEB_3_WS_URL_BSC = Environment.GetEnvironmentVariable("bsc_api_ws_url");


            Web3ClientBSC = new Web3(WEB_3_API_URL_BSC);
            SocketClientBSC = new StreamingWebSocketClient(WEB_3_WS_URL_BSC);

            SocketClientBSC.StartAsync().Wait();


            //Managers AutoManaged Events

            // eth events
            EventsManagerEth = new ManagedEthereumEvents(SocketClientETH, new Web3(WEB_3_WS_URL_ETH));
            // bsc events
            EventsManagerBsc = new ManagedEthereumEvents(SocketClientBSC, new Web3(WEB_3_WS_URL_BSC));
        }

    }
}
