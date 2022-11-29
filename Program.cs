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
            bool loggedIn;
            int userKey = 0;
            do   //looping menu  
            {
                loggedIn = false;
                Console.WriteLine("Enter a number as input to navigate in the menu:");
                Console.WriteLine("-1) Login\n-2) Exit");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        userKey = LogIn(out loggedIn);
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
            } while (loggedIn != true);
            if (DataBase.UserDict[userKey].IsAdmin == true)
            {
                Admin.AdminMenu();
            }
            else
            {
                CustomerMenu(userKey);
            }
        }
        public static int LogIn(out bool loggedIn)
        {
            int userKey = 0;
            loggedIn = false;
            Console.WriteLine("Welcome to KiwiBank");
            Console.WriteLine("Please enter your account name:");
            string userName = Console.ReadLine();

            foreach (KeyValuePair<int, User> item in DataBase.UserDict)
            {
                if (userName == item.Value.UserName)
                {
                    userKey = item.Key;

                    loggedIn = CheckPassWord(userKey);

                    if (loggedIn)
                    {
                        Console.WriteLine("Successfully logged in");
                        Console.WriteLine($"Welcome {userName}");
                        return userKey; //Returns userKey so we know which user is logged in
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
                Console.WriteLine("Password is correct");
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
            RunProgram();
        }

        public static void CustomerMenu(int userKey)
        {
            do   //looping menu  
            {
                Customer obj = (Customer)DataBase.UserDict[userKey];

                Console.WriteLine("Enter a number as input to navigate in the menu:");
                Console.WriteLine("-1) Overview accounts and balances\n-2) Transfer money personal accounts" +
                    "\n-3) Create new account \n-4) Kiwibank internal Transfer money \n-5) Logout");
                string choice = Console.ReadLine();
                
                switch (choice)
                {
                    case "1":
                        obj.AccountOverview();
                        break;
                    case "2":
                        obj.TransferBetweenCustomerAccounts();
                        break;
                    case "3":

                        break;
                    case "4":

                        break;
                    case "5":
                        LogOut();

                        break;

                    default:
                        Console.WriteLine("Wrong input, enter available choice only!");
                        break;
                }
                PressEnterToContinue();
                Console.Clear();// clearing console, 
            } while (true);
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
        public static void PressEnterToContinue()
        {
            Console.WriteLine("Klicka enter för att komma till huvudmenyn");
            ConsoleKey enterPressed = Console.ReadKey(true).Key;
            while (!Console.KeyAvailable && enterPressed != ConsoleKey.Enter)
            {
                enterPressed = Console.ReadKey(true).Key;
            }
        }
    }
}
