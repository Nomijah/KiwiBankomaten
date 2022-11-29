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

        // Used for creating test users
        public Customer(int id, string username, string password)
        {
            Id = id;
            UserName = username;
            Password = password;
            BankAccounts = new Dictionary<int, BankAccount>()
            {
                { 1, new BankAccount("Lönekonto", "SEK", 1m) }
            };
        }

        // Use this when creating users in program
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
            BankAccounts = new Dictionary<int, BankAccount>()
            {
                { 1, new BankAccount("Lönekonto", "SEK", 1m) }
            };
        }

        public void ViewAccounts()
        {
            foreach (KeyValuePair<int, BankAccount> account in BankAccounts)
            {
                Console.WriteLine($"{account.Key}. {account.Value.AccountName}: " +
                    $"{account.Value.Amount} {account.Value.Currency}");
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
        public void AccountOverview()
        {
            int index = 1;
            foreach (var item in BankAccounts.Values)
            {
                Console.WriteLine($"-{index}) -\tKontoNamn : {item.AccountName} -\tKontoSaldo : {item.Amount} {item.Currency}");
                index++;
            }
        }
        public void TransferFromCheck(int transferFromWhichAccount, int transferToWhichAccount, decimal amountMoney)
        {
            if (BankAccounts[transferFromWhichAccount].Amount >= amountMoney)
            {
                TransferFromAccToAcc(transferFromWhichAccount, transferToWhichAccount, amountMoney);
            }
            else
            {
                Console.WriteLine("Not enough money in Account( {0} );\tMoney in Account( {0} ) - {1}", BankAccounts[transferFromWhichAccount].AccountName, BankAccounts[transferFromWhichAccount].Amount);
            }
        }
        public void TransferFromAccToAcc(int transferFromWhichAccount, int transferToWhichAccount, decimal amountMoney)
        {
            BankAccounts[transferFromWhichAccount].Amount -= amountMoney;
            BankAccounts[transferToWhichAccount].Amount += amountMoney;

        }
    }
}
