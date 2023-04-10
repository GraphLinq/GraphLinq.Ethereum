using System;
using System.Collections.Generic;
using System.Text;
using NodeBlock.Engine;
using NodeBlock.Engine.Attributes;
using NodeBlock.Plugin.Ethereum.Nodes.Eth.NftCreator.Models;
using NodeBlock.Plugin.Ethereum.Nodes.Wallet.ManagedWallet;
using static NodeBlock.Plugin.Ethereum.Nodes.Eth.Contracts.StandardNftContract;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using System.Drawing;

namespace NodeBlock.Plugin.Ethereum.Nodes.Eth.NftCreator.Minting
{
	[NodeDefinition("DeployNFTNewMintableNode", "Deploy New Mintable NFT Token", NodeTypeEnum.Deployer, "Blockchain.Ethereum.NFT")]
	[NodeGraphDescription("Deploy on the Ethereum network a new NFT on a specific NFT contract owned by the graph issuer")]
	public class DeployNFTNewMintableNode : Node
	{
		public DeployNFTNewMintableNode(string id, BlockGraph graph)
		 : base(id, graph, typeof(DeployNFTNewMintableNode).Name)
		{
			this.InParameters.Add("wallet", new NodeParameter(this, "wallet", typeof(ManagedWallet), true));
			this.InParameters.Add("nftToken", new NodeParameter(this, "nftToken", typeof(NFTTokenModel), true));
		}

		public NFTTokenModel ntfToken { get; set; }

		public override bool CanBeExecuted => true;

		public override bool CanExecute => true;

		public string getMetadata(string name, string description, string imageUrl)
		{
			var baseTemplate = "{\"title\":\"Asset Metadata\",\"type\":\"object\",\"properties\":{\"name\":{\"type\":\"string\",\"description\":\"{name}\"},\"description\":{\"type\":\"string\",\"description\":\"{description}\"},\"image\":{\"type\":\"string\",\"description\":\"{imageUrl}\"}}}";
			return baseTemplate.Replace("{name}", name).Replace("{description}", description).Replace("{imageUrl}", imageUrl);
		}

		public string saveIPFS(string data, byte[] bytesData = null)
		{
			byte[] bytes = (bytesData == null) ? Encoding.ASCII.GetBytes(data) : bytesData;
			string apiKey = Environment.GetEnvironmentVariable("nft_storage_api_key");

			string url = string.Format("https://api.nft.storage/upload");

			var request = (HttpWebRequest)WebRequest.Create(url);

			request.Method = "POST";
			request.ContentType = "application/x-binary";
			request.ContentLength = bytes.Length;
			request.Headers.Add("Authorization", string.Format("Bearer {0}", apiKey));

			using (var stream = request.GetRequestStream())
			{
				stream.Write(bytes, 0, bytes.Length);
			}
			var response = (HttpWebResponse)request.GetResponse();
			Stream dataStream = response.GetResponseStream();
			StreamReader reader = new StreamReader(dataStream);
			string strResponse = reader.ReadToEnd();
			if (strResponse.Length > 0)
			{
			   dynamic ipfsResponse =  JsonConvert.DeserializeObject<dynamic>(strResponse);
			   if (ipfsResponse.ok == true)
					return ipfsResponse.value.cid;
			}
   
			throw new Exception("Can't save the NFT Metadata to NFT.Storage IPFS, please try again later or contact the GLQ team");
		}



		public byte[] downloadImage(string urlImage)
        {
			using (WebClient webClient = new WebClient())
			{
				return webClient.DownloadData(urlImage);
				
			}
		}

		public override bool OnExecution()
		{
			ManagedWallet wallet = (this.InParameters["wallet"].GetValue() as ManagedWallet);
			var nftToken = (this.InParameters["nftToken"].GetValue() as NFTTokenModel);


			var account = wallet.GetAccount();
			var web3 = new Nethereum.Web3.Web3(account, Environment.GetEnvironmentVariable("eth_api_http_url"));
			var baseIpfsUri = Environment.GetEnvironmentVariable("base_uri_ipfs");
			try
			{
				byte[] imageBytes = downloadImage(nftToken.ImageUrl);
				string cid = saveIPFS(string.Empty, imageBytes);
				string imageUri = string.Format("https://{0}.{1}/", cid, baseIpfsUri);

				string metaData = getMetadata(nftToken.Name, nftToken.Description, imageUri);
				cid = saveIPFS(metaData);
				var mintFunctionMessage = new MintFunction()
				{
					MintTo = nftToken.TokenOwner,
					TokenUri = string.Format("https://{0}.{1}/", cid, baseIpfsUri)
				};

				var mintHandler = web3.Eth.GetContractTransactionHandler<MintFunction>();
				_ = mintHandler.SendRequestAndWaitForReceiptAsync(nftToken.ContractAddress, mintFunctionMessage).Result;
				this.Graph.AppendLog("info", string.Format("NFT \"{0}\" minted on contract {1} by managed wallet {2}, token minted owner -> {3}", nftToken.Name,
					nftToken.ContractAddress, account.Address, nftToken.TokenOwner));
				return true;
			}
			catch (Exception error)
			{
				this.Graph.AppendLog("error", error.ToString());
				return false;
			}
		}
	}
}
