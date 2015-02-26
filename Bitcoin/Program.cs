using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using Info.Blockchain.API.BlockExplorer;
using Info.Blockchain;
using Info.Blockchain.API;
using Info.Blockchain.API.Wallet;
using Info.Blockchain.API.ExchangeRates;
using Address = Info.Blockchain.API.BlockExplorer.Address;

namespace Bitcoin
{
    class Program
    {
        static void Main(string[] args)
        {

            while (true)
            {
                GetWalletBalance();    
                Thread.Sleep(1000);
                Console.Clear();
            }
            
            //UsdToBitcoin(2);
            
            //GetExchangeRates(2);
            
            //GetWalletDetails("1DW29NUoat7ayGpj6Gm45i5BxCxYfm8oD8", true);
            
            //GetTransactionDetails("2bcdd3a51df57a5967dca3450a8f860ff5e9944b28465d31070683cc3ea60299");
            
            //GetWalletBalance();
            //SendMoney();
            //GetWalletBalance();
            
            Console.ReadLine();
        }

        private static void GetWalletBalance()
        {
            Wallet wallet = new Wallet("9610d5cc-e92c-469b-b83a-38b7a538a518", "Hackathon2015");
            Console.WriteLine((double)wallet.GetBalance() / 100000000 + " BTC or " + BitcoinToUsd((double)wallet.GetBalance() / 100000000) + " USD");
        }

        private static void SendMoney()
        {
            Wallet wallet = new Wallet("9610d5cc-e92c-469b-b83a-38b7a538a518", "Hackathon2015");
            PaymentResponse payment = wallet.Send("1CJWA3BzyFJegeu4KA9LwD32opmSuSwtBK",
                    2000000, fee: 500000L, note: "Test");
            Console.WriteLine("The payment TX hash is {0}", payment.TxHash);
        }

        private static void GetTransactionDetails(string transactionId)
        {
            var explorer = new BlockExplorer();
            Transaction trns = explorer.GetTransaction(transactionId);
            Console.WriteLine("********* TRANSACTION DETAILS *********");
            Console.WriteLine("Transaction ID: " + trns.Hash);
            
            foreach (Output o in trns.Outputs)
            {
                Console.WriteLine("BTC Spent : " + (double) o.Value / 100000000);
            }
           
            Console.WriteLine("Included in block: " + trns.BlockHeight);
            Console.WriteLine("Is it double spent? " + trns.DoubleSpend);
            long unixDate = trns.Time;
            DateTime start = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            DateTime date = start.AddSeconds(unixDate).ToLocalTime();
            Console.WriteLine("Transaction time: " + date);
            Console.WriteLine("");
        }

        private static void GetWalletDetails(string walletID, bool includeTransactions)
        {
            var explorer = new BlockExplorer();
            Address address = explorer.GetAddress(walletID);
            double balance = address.FinalBalance;
            balance = balance/100000000;
            Console.WriteLine("********* WALLET DETAILS *********");
            Console.WriteLine("Wallet ID: " + address.AddressStr);
            Console.WriteLine("Balance of address is: " + balance + " BTN");
            Console.WriteLine("");

            if (includeTransactions)
            {
                foreach (Transaction transaction in address.Transactions)
                {
                    GetTransactionDetails(transaction.Hash);
                }
                Console.WriteLine("");    
            }
            
        }

        private static void GetExchangeRates(double bitcoins)
        {
            Dictionary<string,Currency> exchangerates = ExchangeRates.GetTicker();
            foreach (var currency in exchangerates)
            {
                Console.Write(bitcoins + " BTC = ");
                Console.WriteLine(currency.Value.Last*bitcoins + " " + currency.Key);
            }
        }

        private static void UsdToBitcoin(double usd)
        {
            Dictionary<string, Currency> exchangerates = ExchangeRates.GetTicker();
            double exchangeRate = 1/exchangerates["USD"].Last;
            Console.WriteLine(usd + " USD is = " + exchangeRate*usd  + " BTC");
        }

        private static double BitcoinToUsd(double bitcoin)
        {
            Dictionary<string, Currency> exchangerates = ExchangeRates.GetTicker();
            double exchangeRate = exchangerates["USD"].Last;
            return (double) exchangeRate*bitcoin;
        }
    }
}
