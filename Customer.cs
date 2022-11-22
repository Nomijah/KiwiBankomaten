using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KiwiBankomaten
{
    internal class Customer : User
    {
        private List<Account> Accounts;
        public Customer(int id, string username, string password)
        {
            Id = id;
            UserName = username;
            Password = password;
            Accounts = new List<Account>()
            {
                new Account("Bankkonto", "SEK", 1m)
            };
        }

        public Customer(string username, string password)
        {
            int newId = DataBase.CustomerList.Last().Key;
            Id = newId;
            UserName = username;
            Password = password;
            Accounts = new List<Account>()
            {
                new Account("Bankkonto", "SEK", 1m)
            };
        }
    }
}
