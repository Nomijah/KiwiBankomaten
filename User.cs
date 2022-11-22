using System;
using System.Collections.Generic;
using System.Text;

namespace KiwiBankomaten
{
    internal abstract class User
    {





        public void LogIn()
        {



            Console.WriteLine("Welcome to KiwiBank");
            Console.WriteLine("Please enter your account name:");
            int userName = int.Parse(Console.ReadLine());
            int pinCode = int.Parse(Console.ReadLine());




        }

    }



}
