using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace KiwiBankomaten
{
    internal class Customer : User
    {
        private List<Accounts> Accounts;
        public Customer(int id, string username, string password, List<Accounts> accList)
        {
            Id = id;
            UserName = username;
            Password = password;
            Accounts = accList;
        }

    }
}
