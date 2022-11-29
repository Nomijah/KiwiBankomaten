using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;

namespace KiwiBankomaten
{
    internal class Customer : User
    {
        private Dictionary<int, BankAccount> BankAccounts;

        // Used for creating test customers
        public Customer(int id, string username, string password)
        {
            Id = id;
            UserName = username;
            Password = password;
            IsAdmin = false;

            // Bankaccounts for testing, same for each user
            BankAccounts = new Dictionary<int, BankAccount>()
            {
                { 1, new BankAccount("Lönekonto", 25347.34m, "SEK", 0m) },
                { 2, new BankAccount("Sparkonto", 324000m, "SEK", 2.5m) },
                { 3, new BankAccount("Utlandskonto", 74654.36m, "EUR", 1.3m) },
                { 4, new BankAccount("Företagskonto", 624.86m, "USD", 0m) }
            };
        }

        // Use this when creating customers in program
        public Customer(string username, string password)
        {
            if (DataBase.UserDict == null)
            {
                Id = 1;
            }
            else
            {
                int newId = DataBase.UserDict.Last().Key + 1;
                Id = newId;
            }
            UserName = username;
            Password = password;
            IsAdmin = false;
            
            BankAccounts = new Dictionary<int, BankAccount>()
            {
                { 1, new BankAccount("Lönekonto", "SEK", 1m) }
            };
        }

        public override void ViewAccounts()
        {
            foreach (KeyValuePair<int, BankAccount> account in BankAccounts)
            {
                Console.WriteLine($"{account.Value.AccountNumber} {account.Value.AccountName}: " +
                    $"{account.Value.Amount} {account.Value.Currency}");
            }
        }
        public void AccountOverview()
        {
            int index = 1;
            foreach (var item in BankAccounts.Values)
            {
                Console.WriteLine($"-{index}) -\tKontoNamn : {item.AccountName} - KontoSaldo : {item.Amount} {item.Currency}");
                index++;
            }
        }
        public void TransferBetweenCustomerAccounts()
        {
            decimal amountMoney;
            int transferFromWhichAccount;
            int transferToWhichAccount;

            Console.Clear();
            AccountOverview();

            Console.WriteLine("How much money do you want to transfer?: ");
            Program.IsValueNumber(out amountMoney);

            Console.WriteLine("From which account do you want to transfer money from?: ");
            Program.IsValueNumber(out transferFromWhichAccount, 1, BankAccounts.Count);

            Console.WriteLine("From which account do you want to transfer money to?: ");
            Program.IsValueNumber(out transferToWhichAccount, 1, BankAccounts.Count);

            TransferFromCheck(transferFromWhichAccount, transferToWhichAccount, amountMoney); 

        }
        private void TransferFromCheck(int transferFromWhichAccount, int transferToWhichAccount, decimal amountMoney)
        {
            if (BankAccounts[transferFromWhichAccount].Amount >= amountMoney)
            {
                TransferFromAccToAcc(transferFromWhichAccount, transferToWhichAccount, amountMoney);
                Console.WriteLine("The Transfer was a success");
                AccountOverview();
            }
            else
            {
                Console.WriteLine("Not enough money in Account( {0} );\tMoney in Account( {0} ) - {1}", BankAccounts[transferFromWhichAccount].AccountName, BankAccounts[transferFromWhichAccount].Amount);
            }
        }
        private void TransferFromAccToAcc(int transferFromWhichAccount, int transferToWhichAccount, decimal amountMoney)
        {
            BankAccounts[transferFromWhichAccount].Amount -= amountMoney;
            BankAccounts[transferToWhichAccount].Amount += amountMoney;

        }
    }
}
