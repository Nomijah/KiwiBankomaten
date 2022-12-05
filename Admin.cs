﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KiwiBankomaten
{
    internal class Admin : User
    {
        // Used for creating test admins.
        public Admin(int id, string username, string password)
        {
            Id = id;
            UserName = username;
            Password = password;
        }
        // Use this when creating admins in program.
        public Admin(string username, string password)
        {
            if (DataBase.AdminList == null)
            {
                Id = 1;
            }
            else
            {
                int newId = DataBase.AdminList.Count;
                Id = newId;
            }
            UserName = username;
            Password = password;
            IsAdmin = true;
        }
        // Admin method for creating new users.
        public static void CreateNewUser() 
        {
            // Used in the do-while loop to repeat if any input errors are detected.
            bool error; 
            do
            {
                string userName = "";
                string passWord = "";
                error = false;
                Console.Clear();
                Console.WriteLine("Vilken sorts användare vill du skapa?\n-1 Customer\n-2 Admin");
                string userType = Console.ReadLine();
                if (userType == "1" || userType == "2")
                {
                    Console.Clear();
                    Console.WriteLine("Vilket användarnamn ska den nya användaren ha?");
                    userName = Console.ReadLine();
                    Console.Clear();
                    Console.WriteLine("Vilket lösenord ska den nya användaren ha?");
                    passWord = Console.ReadLine();
                    Console.Clear();
                }

                switch (userType)
                {
                    // Adds customer account to CustomerDict with name and password set from user input.
                    case "1": 
                        DataBase.CustomerDict.Add(DataBase.CustomerDict.Last().Key + 1, new Customer(userName, passWord));
                        Console.WriteLine($"Customer {userName} har skapats med nyckeln {DataBase.CustomerDict.Last().Key}");
                        break;
                    // Adds admin account to AdminList with name and password set from user input.
                    case "2": 
                        DataBase.AdminList.Add(new Admin(userName, passWord));
                        Console.WriteLine($"Admin {userName} har skapats.");
                        break;
                    // Loop repeats and switch is run again if none of the correct values are chosen.
                    default:
                        Console.WriteLine("Fel input, skriv in ett korrekt värde");
                        error = true;
                        break;
                }
            } while (error == true);
        }
        // Menu where admin can select different functions.
        public static void AdminMenu() 
        {
            // Used to log admin out if set to false.
            bool loggedIn = true;
            // Loop that runs so long as the admin has not chosen to log out.
            while (loggedIn == true) 
            {
                Console.Clear();
                Console.WriteLine("Funktioner för admins:\n-1 Skapa ny användare\n-2 Uppdatera växlingskurs\n-3 Logga ut");
                switch (Console.ReadLine())
                {
                    case "1":
                        CreateNewUser();
                        break;
                    case "2": //UpdateExchangeRate();
                        break;
                    // User will be broken out of loop and then logged out if 3 is chosen.
                    case "3": loggedIn = false;
                        Console.Clear();
                        break;
                    // Loop repeats and switch is run again if none of the correct values are chosen.
                    default:
                        Console.WriteLine("Fel input, skriv in ett korrekt värde");
                        break;
                }
            }
            // Program.LogOut is called outside the loop and switch because of possible bugs if it were to be called inside it.
            Program.LogOut(); 
        }
    }
}
