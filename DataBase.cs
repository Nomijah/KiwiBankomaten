using System;
using System.Collections.Generic;
using System.Text;

namespace KiwiBankomaten
{
    class DataBase
    {
        public static Dictionary<int, User> CustomerList = 
            new Dictionary<int, User>()
        {
                {1, new Customer(1, "Anas", "Core3.1") },
                {2, new Customer(2, "Tobias", "Qlok") },
                {3, new Customer(3, "Reidar", "Password123") },
                {4, new Customer(4, "Michael", "abc") },
                {5, new Customer(5, "Andre", "123") },
                {6, new Customer(6, "Ludvig", "drowssaP") }
        };
    }
}
