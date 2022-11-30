using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KiwiBankomaten
{
    internal class Admin : User
    {
        // Used for creating test admins
        public Admin(int id, string username, string password)
        {
            Id = id;
            UserName = username;
            Password = password;
        }
        // Use this when creating admins in program
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
        public static void CreateNewUser()
        {
            bool error;
            do
            {
                error = false;
                Console.Clear();
                Console.WriteLine("Vilken sorts användare vill du skapa?\n-1 Customer\n-2 Admin");
                string userType = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Vilket användarnamn ska den nya användaren ha?");
                string userName = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Vilket lösenord ska den nya användaren ha?");
                string passWord = Console.ReadLine();
                Console.Clear();

                switch (userType)
                {
                    case "1":
                        DataBase.CustomerDict.Add(DataBase.CustomerDict.Last().Key + 1, new Customer(userName, passWord));
                        Console.WriteLine($"Customer {userName} har skapats med nyckeln {DataBase.CustomerDict.Last().Key}");
                        break;
                    case "2":
                        DataBase.AdminList.Add(new Admin(userName, passWord));
                        Console.WriteLine($"Admin {userName} har skapats.");
                        break;
                    default:
                        Console.WriteLine("Fel input, skriv in ett korrekt värde");
                        error = true;
                        break;
                }
            } while (error == true);
        }
        public static void AdminMenu()
        {
            bool loggedIn = true;
            while (loggedIn == true)
            {
                Console.WriteLine("Funktioner för admins:\n-1 Skapa ny användare\n-2 Uppdatera växlingskurs\n-3 Logga ut");
                switch (Console.ReadLine())
                {
                    case "1":
                        CreateNewUser();
                        break;
                    case "2": //UpdateExchangeRate();
                        break;
                    case "3": loggedIn = false;
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Fel input, skriv in ett korrekt värde");
                        break;
                }
            }
            Program.LogOut();
        }
    }
}
