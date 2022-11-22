using System;
using System.Collections.Generic;
using System.Text;

namespace KiwiBankomaten
{
    class DataBase
    {
        public List<Customer> CustomersList = new List<Customer>()
        {
            new Customer(1, "Anas", "Core3.1", new List<Accounts>()
            {

            }),
            new Customer(2, "Tobias", "Qlok", new List<Accounts>()
            {

            }),
            new Customer(3, "Reidar", "Password123", new List<Accounts>()
            {

            }),
            new Customer(4, "Michael", "abc", new List<Accounts>()
            {
                
            }),
            new Customer(5, "Andre", "123", new List<Accounts>()
            {

            }),
            new Customer(6, "Ludvig", "drowssaP", new List<Accounts>()
            {

            }),
        };
    }
}
