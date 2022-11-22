using System;
using System.Collections.Generic;
using System.Text;

namespace KiwiBankomaten
{
    class DataBase
    {
        public List<Customer> CustomersList = new List<Customer>()
        {
            new Customer(1, "Anas", "Core3.1"),
            new Customer(2, "Tobias", "Qlok"),
            new Customer(3, "Reidar", "Password123"),
            new Customer(4, "Michael", "abc"),
            new Customer(5, "Andre", "123"),
            new Customer(6, "Ludvig", "drowssaP")
        };
    }
}
