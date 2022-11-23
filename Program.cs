using System;
using System.Collections.Generic;

namespace KiwiBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            LogIn();
        }

        //  och menu Runprogram Exitprogram     sparar jag nu så masterbranch



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
    }
}
