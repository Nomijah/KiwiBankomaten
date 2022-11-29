using System;
using System.Collections.Generic;
using System.Threading;

namespace KiwiBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunProgram();
        }
        public static void RunProgram()
        {
            do   //looping menu  
            {
                Console.WriteLine("Enter a number as input to navigate in the menu:");
                Console.WriteLine("-1) Login\n-2) Exit");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        LogIn();
                        break;
                    case "2"://Exit program
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Wrong input, enter available choice only!");
                        break;
                }
                Thread.Sleep(2000);//leaves eventual message readable for 2 sec
                Console.Clear();// clearing console, 
            } while (true);
        }
        public static int LogIn()
        {
            int userKey = 0;
            bool loggedIn = false;
            
            Console.WriteLine("Welcome to KiwiBank");
            Console.WriteLine("Please enter your account name:");
            string userName = Console.ReadLine();
            foreach (KeyValuePair<int, User> item in DataBase.UserDict)
            {
                if (userName == item.Value.UserName)
                {
                    userKey = item.Key;
                    
                    if (loggedIn = CheckPassWord(userKey))
                    {
                        return userKey;
                    }
                }
            }
            return 0;
        }
        public static bool CheckPassWord(int userKey)
        {
            Console.WriteLine("Enter your password");
            string userPassWord = (Console.ReadLine());

            if (userPassWord == DataBase.UserDict[userKey].Password)
            {
                Console.WriteLine("Sucessfully logged in");
                return true;
            }
            else
            {
                Console.WriteLine("Wrong password");
                return false;
            }
        }

        public static void LogOut() 
        {
            LogIn(); //Tänker att om man kör Logoutmetoden så ska man komma tillbaka till loginskärmen

        }
        public static void IsValueNumber(out decimal amountMoney)
        {
            while (!decimal.TryParse(Console.ReadLine(), out amountMoney)) //How much money is being transferred
            {
                Console.WriteLine("Please input a Number: ");
            }
        }
        public static void IsValueNumber(out int amountMoney, int minValue, int maxValue)
        {
            while (!int.TryParse(Console.ReadLine(), out amountMoney) && amountMoney >= minValue && amountMoney <= maxValue) //How much money is being transferred
            {
                Console.WriteLine("Please input a Number between {0} and {1} : ", minValue, maxValue);
            }
        }
    }
}
