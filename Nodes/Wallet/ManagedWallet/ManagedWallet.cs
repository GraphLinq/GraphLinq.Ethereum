using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using NBitcoin;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Util;
using Nethereum.Hex.HexConvertors.Extensions;
using Nethereum.HdWallet;
using System.Security.Cryptography;
using System.IO;
using NodeBlock.Engine.Attributes;

namespace NodeBlock.Plugin.Ethereum.Nodes.Wallet.ManagedWallet
{
    [ExportableObject("ManagedEthereumWallet")]
    public class ManagedWallet
    {
        public ManagedWallet(Storage.Entities.ManagedWallet managedWalletEntity)
        {
            ManagedWalletEntity = managedWalletEntity;
            Gwei = 1;
        }

        public Storage.Entities.ManagedWallet ManagedWalletEntity { get; }
        public int Gwei { get; set; }

        public static Tuple<string, string> GenerateNewAddress(string password)
        {
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.Key = Encoding.Default.GetBytes(Environment.GetEnvironmentVariable("managed_wallet_key"));
                rijndael.IV = Encoding.Default.GetBytes(Environment.GetEnvironmentVariable("managed_wallet_key_iv"));
                ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);
                Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
                var wallet = new Nethereum.HdWallet.Wallet(mnemo.ToString(), password);
                var privateKey = wallet.GetPrivateKey(0);

                return new Tuple<string, string>(wallet.GetAddresses(1)[0], privateKey.ToString());
            }  
        }

        public static ManagedWallet GetOrCreateManagedWallet(int walletId, string name, string password)
        {
            using (var scope = Plugin.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetService<Storage.DatabaseStorage>();
                var walletEntity = context.FetchManagedWalletByName(walletId, name);
                if(walletEntity == null)
                {
                    using (RijndaelManaged rijndael = new RijndaelManaged())
                    {
                        rijndael.Key = Encoding.Default.GetBytes(Environment.GetEnvironmentVariable("managed_wallet_key"));
                        rijndael.IV = Encoding.Default.GetBytes(Environment.GetEnvironmentVariable("managed_wallet_key_iv"));
                        ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);
                        Mnemonic mnemo = new Mnemonic(Wordlist.English, WordCount.Twelve);
                        var wallet = new Nethereum.HdWallet.Wallet(mnemo.ToString(), password);

                        // Encrypt the private key
                        var privateKeyEncrypted = string.Empty;
                        using (MemoryStream msEncrypt = new MemoryStream())
                        {
                            using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            {
                                using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                                {
                                    swEncrypt.Write(wallet.GetPrivateKey(0).ToHex());
                                }
                                privateKeyEncrypted = msEncrypt.ToArray().ToHex();
                            }
                        }

                        // Create a new managed wallet if this one doesnt exist
                        walletEntity = new Storage.Entities.ManagedWallet()
                        {
                            WalletId = walletId,
                            Name = name,
                            PublicKey = wallet.GetAddresses(1)[0],
                            PrivateKey = privateKeyEncrypted,
                            Password = password,
                            CreatedAt = DateTime.UtcNow,
                            UpdatedAt = DateTime.UtcNow
                        };
                        context.Add(walletEntity);
                        context.SaveChanges();
                    }
                }
                return new ManagedWallet(walletEntity);
            }
        }

        public string GetPrivateKey()
        {
            using (RijndaelManaged rijndael = new RijndaelManaged())
            {
                rijndael.Key = Encoding.Default.GetBytes(Environment.GetEnvironmentVariable("managed_wallet_key"));
                rijndael.IV = Encoding.Default.GetBytes(Environment.GetEnvironmentVariable("managed_wallet_key_iv"));
                ICryptoTransform encryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);
                var privateKey = string.Empty;
                using (MemoryStream msEncrypt = new MemoryStream(this.ManagedWalletEntity.PrivateKey.HexToByteArray()))
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader swEncrypt = new StreamReader(csEncrypt))
                        {
                            privateKey = swEncrypt.ReadToEnd();
                        }
                    }
                }
                return privateKey;
            }
        }

        public Account GetAccount()
        {
            return new Account(this.GetPrivateKey());
        }
    }
}
