using System;
using System.Threading.Tasks;
using Nethereum.JsonRpc.WebSocketStreamingClient;
using Nethereum.Web3;
using Newtonsoft.Json;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;

namespace NodeBlock.Plugin.Ethereum.Nodes.BSC
{
    [NodeDefinition("BSCConnectorNode", "Binance Smart Chain Connector", NodeTypeEnum.Connector, "Blockchain.BSC")]
    [NodeGraphDescription("Connection to the Binance Smart Chain network, can be used as Managed connection (without in parameters) or with your own node")]
    public class BSCConnectorNode : Node
    {
        public BSCConnectorNode(string id, BlockGraph graph)
            : base(id, graph, typeof(BSCConnectorNode).Name)
        {
            this.CanBeSerialized = false;

            this.InParameters.Add("url", new NodeParameter(this, "url", typeof(string), true));
            this.InParameters.Add("socketUrl", new NodeParameter(this, "socketUrl", typeof(string), true));

            this.OutParameters.Add("connection", new NodeParameter(this, "connection", typeof(BSCConnectorNode), true));
        }
        [JsonIgnore]
        public Web3 Web3Client { get; set; }

        [JsonIgnore]
        public StreamingWebSocketClient SocketClient { get; set; }

        [JsonIgnore]
        public bool UseManaged = false;

        public override bool CanBeExecuted => false;

        public override bool CanExecute => true;

        public override void SetupConnector()
        {
            if(this.InParameters["url"].GetValue() == null || this.InParameters["socketUrl"].GetValue() == null)
            {
                UseManaged = true;

                this.Web3Client = Plugin.Web3ClientBSC;
                this.SocketClient = Plugin.SocketClientBSC;
            }
            else
            {
                UseManaged = false;
                this.Web3Client = new Web3(this.InParameters["url"].GetValue().ToString());
                this.SocketClient = new StreamingWebSocketClient(this.InParameters["socketUrl"].GetValue().ToString());
                this.SocketClient.StartAsync().Wait();
            }
            this.Next();
        }

        public override void OnStop()
        {
            if (!UseManaged) 
                this.SocketClient.StopAsync().Wait();
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