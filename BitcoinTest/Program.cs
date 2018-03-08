using System;
using NBitcoin;
using NBitcoin.Crypto;
using NBitcoin.Stealth;
using QBitNinja.Client;
using QBitNinja.Client.Models;
using System.Threading;
using NBitcoin.Protocol;

// using System.Collections.Generic;
// address = n3GhhFRs2PhR5WoaewcoVd1AmCpTDZrQY2
// privKey = cQUHRsd1RboEjK96wAfDMXby6yy7Wve2TpMvBHbPuAUksxje4brN


namespace BitcoinTest
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            TestDemo();
            Console.WriteLine("Hello World!");
        }

        public static void TestDemo() 
        {
            Key privKey = new Key();
            // var demoSecret = privKey.GetWif(Network.TestNet);
            var demoSecret = new BitcoinSecret("cQUHRsd1RboEjK96wAfDMXby6yy7Wve2TpMvBHbPuAUksxje4brN");
            BitcoinPubKeyAddress address = demoSecret.GetAddress();

            string txId = "97967e6bef942ffc75ff920292510eddbd8665b6bb7382f95c63fff735a9cc23";
            QBitNinjaClient client = new QBitNinjaClient(Network.TestNet);
            var txResponse = client.GetTransaction(new uint256(txId)).Result;

            // Define the input transaction
            TxIn newInput = new TxIn(txResponse.ReceivedCoins[0].Outpoint, demoSecret.GetAddress().ScriptPubKey);

            // Define the output transactions
            TxOut newOut = new TxOut();
            int outputSatoshis = 25000;
            Money newOutputToSend = Money.Satoshis(outputSatoshis);
            newOut.Value = newOutputToSend;

            string toAddress = "mg2b7TtbGhvn3rEjqabAdM594MzczbCy7a";
            var newDestination = BitcoinAddress.Create(toAddress);
            newOut.ScriptPubKey = newDestination.ScriptPubKey;

            // Create the transaction
            Transaction newTransaction = new Transaction();
            newTransaction.AddInput(newInput);
            newTransaction.AddOutput(newOut);
            newTransaction.Sign(demoSecret, txResponse.ReceivedCoins[0]);

            var node = Node.Connect(Network.TestNet, "54.241.159.189:18333");
            node.VersionHandshake();
            node.SendMessage(new InvPayload(newTransaction));
            node.SendMessage(new TxPayload(newTransaction));
            Thread.Sleep(4000);
            node.Disconnect();

               
            int a = 6;
        }
    }
}
