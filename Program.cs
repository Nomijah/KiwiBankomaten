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



        public static void LogIn()
        {
            Console.WriteLine("Welcome to KiwiBank");
            Console.WriteLine("Please enter your account name:");
            string userName = (Console.ReadLine());
            foreach (KeyValuePair<int, User> item in DataBase.CustomerList)
            {
                if (userName == item.Value.UserName)
                {
                    int userKey = item.Key;
                    CheckPassWord(userKey);
                }
            }
        }
        public static void CheckPassWord(int userKey)
        {
            Console.WriteLine("Enter your password");
            string userPassWord = (Console.ReadLine());

            if (userPassWord == DataBase.CustomerList[userKey].Password)
            {
                Console.WriteLine("congratz petter was right");
            }
        }
    }
}
