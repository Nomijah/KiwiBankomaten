﻿using System;
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
        public static void LogIn()
        {
            Console.WriteLine("Welcome to KiwiBank");
            Console.WriteLine("Please enter your account name:");
            string userName = (Console.ReadLine());
            foreach (KeyValuePair<int, User> item in DataBase.UserDict)
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

            if (userPassWord == DataBase.UserDict[userKey].Password)
            {
                Console.WriteLine("congratz petter was right");
            }
        }
    }
}
