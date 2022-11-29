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
                //Customer Menu here
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
                    
                    if (loggedIn = CheckPassWord(userKey))
                    {
                        loggedIn = true;
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
            RunProgram(); //Tänker att om man kör Logoutmetoden så ska man komma tillbaka till loginskärmen
        }
    }
}
