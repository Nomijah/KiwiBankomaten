using System;
using System.Collections.Generic;
using System.Text;

namespace KiwiBankomaten
{
    internal class Customer : User
    {
        List<Accounts> Accounts;
        public Customer(int id, string username, string password, List<Accounts> accList)
        {
            ID = id;
            UserName = username;
            Password = password;
            Accounts = accList;
        }
    }
}
