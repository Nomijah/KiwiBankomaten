using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KiwiBankomaten
{
    internal class Customer : User
    {
        private List<BankAccount> BankAccounts;

        // Used for creating test users
        public Customer(int id, string username, string password)
        {
            Id = id;
            UserName = username;
            Password = password;
            BankAccounts = new List<BankAccount>()
            {
                new BankAccount("Lönekonto", "SEK", 1m)
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
            BankAccounts = new List<BankAccount>()
            {
                new BankAccount("Lönekonto", "SEK", 1m)
            };
        }
    }
}
