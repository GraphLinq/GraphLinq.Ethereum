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
using static NodeBlock.Plugin.Ethereum.Nodes.Uniswap.Entities.UniswapPair;
using Nethereum.Contracts;
using Nethereum.RPC.Reactive.Eth.Subscriptions;
using Nethereum.JsonRpc.WebSocketClient;
using Newtonsoft.Json;
using static NodeBlock.Plugin.Ethereum.Nodes.BSC.PankakeSwap.Entities.PancakeSwapFactory;
using static NodeBlock.Plugin.Ethereum.Nodes.BSC.PankakeSwap.Entities.PankakeSwapPairContract;

namespace NodeBlock.Plugin.Ethereum.Nodes
{
    public class CustomUniswapSwapEvent : EthLogsSubscription
    {
        public EthLogsSubscription Subscription;
        public CustomUniswapSwapEvent(IStreamingClient client) : base(client)
        {

        }
    }

    public class CustomUniswapSyncEvent : EthLogsSubscription
    {
        public EthLogsSubscription Subscription;
        public CustomUniswapSyncEvent(IStreamingClient client) : base(client)
        {

        }
    }

    public class PancakeSwapNewPairEvent : EthLogsSubscription
    {
        public EthLogsSubscription Subscription;
        public PancakeSwapNewPairEvent(IStreamingClient client) : base(client)
        {

        }
    }

    public class PancakeSwapNewSwapEvent : EthLogsSubscription
    {
        public EthLogsSubscription Subscription;
        public PancakeSwapNewSwapEvent(IStreamingClient client) : base(client)
        {

        }
    }

    public class ManagedEthereumEvents
    {
        public ConcurrentDictionary<string, Tuple<dynamic, ConcurrentDictionary<BlockGraph, IEventEthereumNode>>> Events;
        public StreamingWebSocketClient SocketClient { get; set; }
        public Web3 Web3Client { get; set; }

        public static bool ManagerAlive = true;

        public ManagedEthereumEvents(StreamingWebSocketClient socket, Web3 web3Client)
        {
            SocketClient = socket;
            Web3Client = web3Client;
            Events = new ConcurrentDictionary<string, Tuple<dynamic, ConcurrentDictionary<BlockGraph, IEventEthereumNode>>>();
            IsSubscribedLoop();
            CheckWebSocketIsAlive();
        }

        private void CheckWebSocketIsAlive()
        {
            new Task(async () =>
            {
                while (ManagerAlive)
                {

                    try
                    {
                        var subscription = new EthNewBlockHeadersObservableSubscription(SocketClient);
                        DateTime? lastBlockNotification = null;
                        double secondsSinceLastBlock = 0;

                        subscription.GetSubscriptionDataResponsesAsObservable().Subscribe(block =>
                        {
                            secondsSinceLastBlock = (lastBlockNotification == null) ? 0 : (int)DateTime.Now.Subtract(lastBlockNotification.Value).TotalSeconds;
                            lastBlockNotification = DateTime.Now;
                            var utcTimestamp = DateTimeOffset.FromUnixTimeSeconds((long)block.Timestamp.Value);
                        });

                        // subscribe
                        await subscription.SubscribeAsync();

                        // run for a minute before unsubscribing
                        await Task.Delay(TimeSpan.FromMinutes(3));

                        try
                        {
                            // unsubscribe
                            await subscription.UnsubscribeAsync();
                        }
                        catch (Exception error)
                        {
                            Console.WriteLine(error);
                        }

                        if (lastBlockNotification.HasValue)
                        {
                            DateTime now = DateTime.Now;
                            double seconds = now.Subtract(lastBlockNotification.Value).TotalSeconds;
                            if (seconds > 180)
                            {
                                Console.WriteLine("Managed events: No new block since 180 seconds, restarting engine..");
                                Environment.Exit(-1);
                            }
                        }
                    }
                    catch (Exception error)
                    {
                        Console.WriteLine(error);
                        Environment.Exit(-1);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
            }).Start();
        }

        private void IsSubscribedLoop()
        {
            new Task(() =>
            {
                while (ManagerAlive)
                {
                    if (Events != null)
                    {
                        Events.ToList().ForEach(x =>
                        {
                            var attachedNodes = x.Value.Item2.ToList().Select(y => y.Key.IsRunning).Count();
                            if (attachedNodes == 0) { ResetEvent(x.Key, x.Value.Item1); }
                        });
                    }

                    System.Threading.Thread.Sleep(TimeSpan.FromSeconds(10));
                }
            }).Start();
        }


        public void RemoveEventNode(string eventType, IEventEthereumNode node)
        {
            try
            {
                Tuple<dynamic, ConcurrentDictionary<BlockGraph, IEventEthereumNode>> wrapperEvent;

                Events.TryGetValue(eventType, out wrapperEvent);
                wrapperEvent.Item2.TryRemove(node.Graph, out _);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }

        public void AddEventNode(string eventType, IEventEthereumNode node)
        {
            try
            {
                Tuple<dynamic, ConcurrentDictionary<BlockGraph, IEventEthereumNode>> wrapperEvent;

                Events.TryGetValue(eventType, out wrapperEvent);
                wrapperEvent.Item2.TryAdd(node.Graph, node);
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }

        private void ResetEvent(string type, dynamic eventObject)
        {
            try
            {
                eventObject.UnsubscribeAsync().Wait();
                Events.Remove(type, out _);

                Console.WriteLine(string.Format("[Managed Ethereum]: '{0}' event cleared, no new nodes attached to this event", type));
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }

        private void OnEvent(object sender, dynamic e)
        {
            string type = sender.GetType().ToString();
            try
            {
                Tuple<dynamic, ConcurrentDictionary<BlockGraph, IEventEthereumNode>> wrapperEvent;
                Events.TryGetValue(type, out wrapperEvent);
                if (wrapperEvent == null) return;

                wrapperEvent.Item2.ToList().ForEach(x =>
                {
                    var node = x.Value;
                    if (node.Graph != null && node.Graph.IsRunning)
                        new Task(() => node.OnEventNode(sender, e)).Start();
                    else
                    {
                        RemoveEventNode(type, node);
                    }
                });
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }

        public EthNewPendingTransactionSubscription NewEventTypePendingTxs(IEventEthereumNode attachedNode)
        {
            EthNewPendingTransactionSubscription newEvent = new EthNewPendingTransactionSubscription(SocketClient);
            string type = newEvent.GetType().ToString();
            if (!Events.ContainsKey(type))
            {
                newEvent.SubscriptionDataResponse += OnEvent;
                newEvent.SubscribeAsync().Wait();
            }

            AddNodeEvent(attachedNode, newEvent);
            return newEvent;
        }

        public EthNewBlockHeadersObservableSubscription NewEventTypePendingBlocks(IEventEthereumNode attachedNode)
        {
            EthNewBlockHeadersObservableSubscription newEvent = new EthNewBlockHeadersObservableSubscription(SocketClient);
            string type = newEvent.GetType().ToString();
            if (!Events.ContainsKey(type))
            {
                newEvent.GetSubscriptionDataResponsesAsObservable().Subscribe(Block =>
                {
                    OnEvent(newEvent, Block);
                });
                newEvent.SubscribeAsync();
            }

            AddNodeEvent(attachedNode, newEvent);
            return newEvent;
        }

        public PancakeSwapNewPairEvent NewEventTypePancakeSwapNewPair(IEventEthereumNode attachedNode)
        {
            PancakeSwapNewPairEvent newEvent = new PancakeSwapNewPairEvent(SocketClient);
            string type = newEvent.GetType().ToString();
            if (!Events.ContainsKey(type))
            {
                var filterTransfers = Event<PairCreatedEventDTOBase>.GetEventABI().CreateFilterInput();
                newEvent.SubscriptionDataResponse += OnEvent;
                newEvent.SubscribeAsync(filterTransfers).Wait();
            }

            AddNodeEvent(attachedNode, newEvent);
            return newEvent;
        }

        public PancakeSwapNewSwapEvent NewEventTypePancakeSwapNewSwap(IEventEthereumNode attachedNode)
        {
            PancakeSwapNewSwapEvent newEvent = new PancakeSwapNewSwapEvent(SocketClient);
            string type = newEvent.GetType().ToString();
            if (!Events.ContainsKey(type))
            {
                var filterTransfers = Event<BSC.PankakeSwap.Entities.PankakeSwapPairContract.SwapEventDTOBase>.GetEventABI().CreateFilterInput();
                newEvent.SubscriptionDataResponse += OnEvent;
                newEvent.SubscribeAsync(filterTransfers).Wait();
            }

            AddNodeEvent(attachedNode, newEvent);
            return newEvent;
        }

        

        public CustomUniswapSwapEvent NewEventTypeUniswapSwap(IEventEthereumNode attachedNode)
        {
            CustomUniswapSwapEvent newEvent = new CustomUniswapSwapEvent(SocketClient);
            string type = newEvent.GetType().ToString();
            if (!Events.ContainsKey(type))
            {
                var filterTransfers = Event<Uniswap.Entities.UniswapPair.SwapEventDTOBase>.GetEventABI().CreateFilterInput();
                newEvent.SubscriptionDataResponse += OnEvent;
                newEvent.SubscribeAsync(filterTransfers).Wait();
            }


            AddNodeEvent(attachedNode, newEvent);
            return newEvent;
        }

        public CustomUniswapSyncEvent NewEventTypeUniswapSync(IEventEthereumNode attachedNode)
        {
            CustomUniswapSyncEvent newEvent = new CustomUniswapSyncEvent(SocketClient);
            string type = newEvent.GetType().ToString();
            if (!Events.ContainsKey(type))
            {
                var filterTransfers = Event<Uniswap.Entities.UniswapPair.SyncEventDTOBase>.GetEventABI().CreateFilterInput();
                newEvent.SubscriptionDataResponse += OnEvent;
                newEvent.SubscribeAsync(filterTransfers).Wait();
            }


            AddNodeEvent(attachedNode, newEvent);
            return newEvent;
        }


        public void AddNodeEvent(IEventEthereumNode attachedNode, dynamic newEvent)
        {
            string type = newEvent.GetType().ToString();

            try
            {
                if (!Events.ContainsKey(type))
                {

                    var list = new ConcurrentDictionary<BlockGraph, IEventEthereumNode>() { };
                    list.TryAdd(attachedNode.Graph, attachedNode);

                    var wrapper = new Tuple<dynamic, ConcurrentDictionary<BlockGraph, IEventEthereumNode>>(newEvent, list);
                    Events.TryAdd(type, wrapper);
                }
                else
                {
                    AddEventNode(type, attachedNode);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error);
            }
        }
    }

}
