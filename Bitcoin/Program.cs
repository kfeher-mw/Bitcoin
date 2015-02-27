using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
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
            string yn = "";
            int i = 0;
            double transactionBtc;
            double exchangeRate;
            double transactionCompleteCheck;
            double currentWalletBalance;
            double walletBalance;
            Wallet wallet = new Wallet("9610d5cc-e92c-469b-b83a-38b7a538a518", "Hackathon2015");

            currentWalletBalance = (double)wallet.GetBalance() / 100000000;

            Console.WriteLine("Please enter the transaction total in USD");

            double transactionTotal;
            if (!double.TryParse(Console.ReadLine(), out transactionTotal))
            {
                Console.WriteLine("Error converting user input to double");
            }

            UsdToBitcoin(transactionTotal);

            yn = Console.ReadLine();

            if (yn == "y")
            {

                Console.WriteLine("---------------------------------------------");
                Console.WriteLine("----    Genius will now display a        ----");
                Console.WriteLine("---  QR code which the customer will scan ---");
                Console.WriteLine("---------------------------------------------");

                Process.Start("C:\\Users\\matthewc\\Desktop\\bitCoinAddress.PNG");

                Dictionary<string, Currency> exchangerates = ExchangeRates.GetTicker();
                exchangeRate = 1 / exchangerates["USD"].Last;

                transactionBtc = exchangeRate * transactionTotal;

                transactionCompleteCheck = currentWalletBalance + transactionBtc;

                while (i < 30)
                {
                    walletBalance = (double)wallet.GetBalance() / 100000000;
                    Console.WriteLine("Waiting for payment");
                    Console.WriteLine();
                    Console.WriteLine();
                    Thread.Sleep(1000);
                    i++;


                    if (Math.Abs(walletBalance - transactionCompleteCheck) < 0.0001)
                    {
                        Console.WriteLine("*************************");
                        Console.WriteLine("Payment received");
                        Console.WriteLine("*************************");
                        i = 31;
                        Console.Read();
                    }
                    else if (i == 30)
                    {
                        Console.WriteLine("*************************");
                        Console.WriteLine("Transaction timeout - please try again!");
                        Console.WriteLine("*************************");
                        Console.Read();
                    }

                }


            }
            else
            {
                Console.Clear();
            }

        }





        /*
        GetWalletBalance();    
        Thread.Sleep(1000);
        Console.Clear();
            
        UsdToBitcoin(2);
            
        GetExchangeRates(2);
            
        GetWalletDetails("1DW29NUoat7ayGpj6Gm45i5BxCxYfm8oD8", true);
            
        GetTransactionDetails("2bcdd3a51df57a5967dca3450a8f860ff5e9944b28465d31070683cc3ea60299");
            
        GetWalletBalance();
        SendMoney();
       GetWalletBalance();
            
      Console.ReadLine();
    }

    private static void GetWalletBalance()
    {
        Wallet wallet = new Wallet("9610d5cc-e92c-469b-b83a-38b7a538a518", "Hackathon2015");
        Console.WriteLine("Your wallet balance is: " + (double)wallet.GetBalance() / 100000000 + " BTC or " + BitcoinToUsd((double)wallet.GetBalance() / 100000000) + " USD");
    }

    private static void SendMoney()
    {
        Wallet wallet = new Wallet("9610d5cc-e92c-469b-b83a-38b7a538a518", "Hackathon2015");
        Console.WriteLine("Please enter the transaction total in USD");
        double transactionTotal = 0.00;
        transactionTotal = Console.Read();
        PaymentResponse payment = wallet.Send("12j5AWTyFAicifNXbgNNti6DXNS46vC7TG", 
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
*/
        private static void UsdToBitcoin(double usd)
        {
            Dictionary<string, Currency> exchangerates = ExchangeRates.GetTicker();
            double exchangeRate = 1 / exchangerates["USD"].Last;
            Console.WriteLine(usd + " USD is = " + exchangeRate * usd + " BTC  is this ok? (y/n)");
        }
    }
}
        /*
        public static double BitcoinToUsd(double bitcoin)
        {
            Dictionary<string, Currency> exchangerates = ExchangeRates.GetTicker();
            double exchangeRate = exchangerates["USD"].Last;
            
            return (double) exchangeRate*bitcoin;
        }
    }
             
}
*/