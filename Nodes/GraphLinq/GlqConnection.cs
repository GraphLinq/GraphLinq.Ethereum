using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace NodeBlock.Plugin.Ethereum.Nodes.GraphLinq
{
    [NodeDefinition("GlqConnection", "GLQ Connector", NodeTypeEnum.Connector, "Blockchain.GraphLinq")]
    [NodeGraphDescription("Connection to the GraphLinq Chain network, can be used as Managed connection (without in parameters) or with your own node")]
    public class GlqConnection : Node
    {
        public GlqConnection(string id, BlockGraph graph)
            : base(id, graph, typeof(GlqConnection).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("url", new NodeParameter(this, "url", typeof(string), true));
            //this.InParameters.Add("socketUrl", new NodeParameter(this, "socketUrl", typeof(string), true));

            this.OutParameters.Add("connection", new NodeParameter(this, "connection", typeof(GlqConnection), true));
        }

        [JsonIgnore]
        public Web3 Web3Client { get; set; }

        //[JsonIgnore]
        //public StreamingWebSocketClient SocketClient { get; set; }

        [JsonIgnore]
        public bool UseManaged = false;

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            if (this.InParameters["url"].GetValue() == null)
            {
                UseManaged = true;

                this.Web3Client = Plugin.Web3ClientGRAPHLINQ;
                //this.SocketClient = Plugin.SocketClientETH;

            }
            else
            {
                UseManaged = false;
                this.Web3Client = new Web3(this.InParameters["url"].GetValue().ToString());
                //this.SocketClient = new StreamingWebSocketClient(this.InParameters["socketUrl"].GetValue().ToString());
                //this.SocketClient.StartAsync().Wait();
            }
            this.Next();
        }

        public Web3 InstanciateWeb3Account(Account account)
        {
            if (this.InParameters["url"].GetValue() == null)
            {
                return new Web3(account, Plugin.Web3ClientGRAPHLINQ.Client);
            }
            else
            {
                return new Web3(account, this.Web3Client.Client);
            }
        }

        public override void OnStop()
        {
            //if (!UseManaged)
            //    this.SocketClient.StopAsync().Wait();
        }

        public override object ComputeParameterValue(NodeParameter parameter, object value)
        {
            if (parameter.Name == "connection")
            {
                return this;
            }
            return base.ComputeParameterValue(parameter, value);
        }
    }
}
