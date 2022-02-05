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
using Microsoft.Extensions.DependencyInjection;
using NodeBlock.Engine.Storage.MariaDB;
using Microsoft.EntityFrameworkCore;

namespace NodeBlock.Plugin.Ethereum
{
    public class Plugin : BasePlugin
    {
        public static string WEB_3_API_URL_ETH = "";
        public static string WEB_3_WS_URL_ETH = "";

        public static string WEB_3_API_URL_BSC = "";
        public static string WEB_3_WS_URL_BSC = "";

        public static string WEB_3_API_URL_POLYGON = "";
        public static string WEB_3_WS_URL_POLYGON = "";

        public static object mutex = new object();

        public static Web3 Web3ClientETH { get; set; }
        public static StreamingWebSocketClient SocketClientETH { get; set; }

        public static Web3 Web3ClientBSC { get; set; }
        public static StreamingWebSocketClient SocketClientBSC { get; set; }

        public static Web3 Web3ClientPOLYGON { get; set; }
        public static StreamingWebSocketClient SocketClientPOLYGON { get; set; }

        public static ManagedEthereumEvents EventsManagerEth { get; set; }
        public static ManagedEthereumEvents EventsManagerBsc { get; set; }
        public static ManagedEthereumEvents EventsManagerPolygon { get; set; }

        public static bool PluginAlive = true;

        public static ServiceProvider Services;

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public override void Load()
        {
            // ETH
            WEB_3_API_URL_ETH = Environment.GetEnvironmentVariable("eth_api_http_url");
            WEB_3_WS_URL_ETH = Environment.GetEnvironmentVariable("eth_api_ws_url");

            Web3ClientETH = new Web3(WEB_3_API_URL_ETH);
            SocketClientETH = new StreamingWebSocketClient(WEB_3_WS_URL_ETH);

            try
            {
                SocketClientETH.StartAsync().Wait();
                logger.Info("Success! Connected to ETH network");
            }
            catch(Exception exception)
            {
                logger.Error("Failed connecting to ETH network: {0}", exception.Message);
            }


            // BSC
            WEB_3_API_URL_BSC = Environment.GetEnvironmentVariable("bsc_api_http_url");
            WEB_3_WS_URL_BSC = Environment.GetEnvironmentVariable("bsc_api_ws_url");

            Web3ClientBSC = new Web3(WEB_3_API_URL_BSC);
            SocketClientBSC = new StreamingWebSocketClient(WEB_3_WS_URL_BSC);

            try
            {
                SocketClientBSC.StartAsync().Wait();
                logger.Info("Success! Connected to BSC network");
            }
            catch(Exception exception)
            {
                logger.Error("Failed connecting to BSC network: {0}", exception.Message);
            }


            // Polygon (MATIC)
            WEB_3_API_URL_POLYGON = Environment.GetEnvironmentVariable("polygon_api_http_url");
            WEB_3_WS_URL_POLYGON = Environment.GetEnvironmentVariable("polygon_api_ws_url");

            Web3ClientPOLYGON = new Web3(WEB_3_API_URL_POLYGON);
            SocketClientPOLYGON = new StreamingWebSocketClient(WEB_3_WS_URL_POLYGON);

            try
            {
                SocketClientPOLYGON.StartAsync().Wait();
                logger.Info("Success! Connected to POLYGON network");
            }
            catch(Exception exception)
            {
                logger.Error("Failed connecting to POLYGON network: {0}", exception.Message);

            }

            // Init database plugin
            Services = new ServiceCollection()
                .AddScoped(provider => provider.GetService<Storage.DatabaseStorage>())
                .AddDbContextPool<Storage.DatabaseStorage>(options =>
                {
                    options.UseMySQL(
                        Environment.GetEnvironmentVariable("mariadb_uri"));
                })
                .BuildServiceProvider();


            //Managers AutoManaged Events

            // eth events
            EventsManagerEth = new ManagedEthereumEvents(SocketClientETH, new Web3(WEB_3_WS_URL_ETH));
            // bsc events
            EventsManagerBsc = new ManagedEthereumEvents(SocketClientBSC, new Web3(WEB_3_WS_URL_BSC));
            // polygon events
            EventsManagerPolygon = new ManagedEthereumEvents(SocketClientPOLYGON, new Web3(WEB_3_WS_URL_POLYGON));
        }

    }
}
