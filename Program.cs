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
            RunProgram(); // Makes the program go back to the log in menu
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
                        obj.AccountOverview(); // Overviews the Accounts and their respective balances
                        break;
                    case "2":
                        obj.TransferBetweenCustomerAccounts(); // Transfers a value between two accounts the user possesses
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
       
        public static void IsValueNumberCheck(out decimal amountMoney, bool isValueNumberCheck) // Checks whether or not "decimal amountMoney" is valid input
        {
            // amountMoney == The money that is to be transfered and recieved // isValueNumberCheck == If the user input is correct
            do
            {
                if (decimal.TryParse(Console.ReadLine(), out amountMoney) && amountMoney > 0) // Gets user input => Checks if its a decimal => Checks if its larger than 0
                {
                    isValueNumberCheck = false; // If amountMoney is a valid input
                }
                else if (amountMoney < 0) // Checks if amountMoney is larger than 0
                {
                    Console.WriteLine("Input has to be positive and cannot be 0. Please Try Again!");
                    isValueNumberCheck = true; // If amountMoney is lower or equal to 0
                }
                else // If the answer is not a number or other invalid input
                {
                    Console.WriteLine("Please input a Number!");
                    isValueNumberCheck = true; // If amountMoney is an invalid input
                }

            } while (isValueNumberCheck);
        }
        public static void IsValueNumberCheck(out int transferFromOrToWhichAccount, int minValue, int maxValue, bool isValueNumberCheck) // Checks whether or not "int transferFromOrToWhichAccount" is valid input
        {
            // transferFromOrToWhichAccount == The account from which the money will be removed or added // minValue == The key for the Account at the top of the dictionary // maxValue == The key for the Account at the bottom of the dictionary // isValueNumberCheck == If the user input is correct
            do
            {
                if (int.TryParse(Console.ReadLine(), out transferFromOrToWhichAccount) && minValue <= transferFromOrToWhichAccount && maxValue >= transferFromOrToWhichAccount) // Gets user input => Checks if its a decimal => Checks if it's in the set range
                {
                    isValueNumberCheck = false; // If transferFromOrToWhichAccount is a valid input
                }
                else if (minValue > transferFromOrToWhichAccount || maxValue < transferFromOrToWhichAccount) // Checks if transferFromOrToWhichAccount is in the given range 
                {
                    Console.WriteLine("Please input a Number between {0} and {1}!", minValue, maxValue);
                    isValueNumberCheck = true; // If transferFromOrToWhichAccount is an invalid input
                }
                else // If the answer is not a number or other invalid input
                {
                    Console.WriteLine("Please input a Number!");
                    isValueNumberCheck = true; // If transferFromOrToWhichAccount is an invalid input
                }

            } while (isValueNumberCheck); // Loops if input is invalid
        }
        public static void PressEnterToContinue() // Stops the program until the user presses "Enter"
        {
            Console.WriteLine("Klicka enter för att komma till huvudmenyn");
            ConsoleKey enterPressed = Console.ReadKey(true).Key; // Gets the input from the user
            while (!Console.KeyAvailable && enterPressed != ConsoleKey.Enter) // Loops if the user Presses any button other than "Enter"
            {
                enterPressed = Console.ReadKey(true).Key;
            }
        }
    }
}
