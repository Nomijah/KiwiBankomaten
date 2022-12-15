using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KiwiBankomaten
{
    internal class Log
    {
        public DateTime TimeOfTransfer { get; set; }
        public decimal AmountTransferred { get; set; }
        public string Currency { get; set; }
        public int FromWhichAccount { get; set; }
        public int ToWhichAccount { get; set; }

        // Is used to see if account is receiving or sending money.
        public bool ReceivingMoney { get; set; }

        // Used when reading from file to DataBase.
        public Log(decimal amount, string currency, int fromAcc, DateTime time,
            int toAcc)
        {
            AmountTransferred = amount;
            Currency = currency;
            FromWhichAccount = fromAcc;
            TimeOfTransfer = time;
            ToWhichAccount = toAcc;
        }

        // Used when receiving money from another account.
        public Log(decimal amountTransferred, int fromWhichAccount)
        {
            TimeOfTransfer = DateTime.Now;
            AmountTransferred = amountTransferred;
            foreach (Customer c in DataBase.CustomerDict.Values)
            {
                foreach (BankAccount b in c.BankAccounts.Values)
                {
                    if (fromWhichAccount == b.AccountNumber)
                    {
                        Currency = b.Currency;
                        break;
                    }
                }
            }
            FromWhichAccount = fromWhichAccount;
            ReceivingMoney = true;
        }

        // Used when sending money to another account.
        public Log(decimal amountTransferred, int fromWhichAccount, int toWhichAccount)
        {
            TimeOfTransfer = DateTime.Now;
            AmountTransferred = amountTransferred;
            foreach (Customer c in DataBase.CustomerDict.Values)
            {
                foreach (BankAccount b in c.BankAccounts.Values)
                {
                    if (fromWhichAccount == b.AccountNumber)
                    {
                        Currency = b.Currency;
                        break;
                    }
                }
            }
            FromWhichAccount = fromWhichAccount;
            ToWhichAccount = toWhichAccount;
            ReceivingMoney = false;
        }

        // Prints out the relevant info from the log, different output
        // depending on whether you received or sent money.
        public void PrintLog()
        {
            if (ReceivingMoney)
            {
                Console.WriteLine($"Datum: {TimeOfTransfer}\n" +
                    $"Mängd mottaget: {Utility.AmountDecimal(AmountTransferred)} {Currency}\n" +
                    $"Från konto: {FromWhichAccount}\n" +
                    $"-------------------------------");
            }
            else
            {
                Console.WriteLine($"Datum: {TimeOfTransfer}\n" +
                    $"Mängd skickat: {Utility.AmountDecimal(AmountTransferred)} {Currency}\n" +
                    $"Till konto: {ToWhichAccount}\n" +
                    $"-------------------------------");
            }

        }

    }
}
