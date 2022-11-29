using System;
using System.Collections.Generic;
using System.Text;

namespace KiwiBankomaten
{
    class DataBase
    {
        // Dictionary for saving users. With 6 test users created
        public static Dictionary<int, User> UserDict = 
            new Dictionary<int, User>()
        {
                {1, new Admin(1, "Tobias", "NotionLover65") },
                {2, new Customer(2, "Anas", "Core3.1") },
                {3, new Customer(3, "Reidar", "Password123") },
                {4, new Customer(4, "Michael", "abc") },
                {5, new Customer(5, "Andre", "123") },
                {6, new Customer(6, "Ludvig", "drowssaP") }
        };
    }
}
