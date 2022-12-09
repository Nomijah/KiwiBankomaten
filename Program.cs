﻿using System;
using System.Collections.Generic;

namespace KiwiBankomaten
{
    internal class Program
    {
        static void Main(string[] args )
        {
            //DataSaver test = new DataSaver();
            //DataSaver.DSaver();

            RunProgram();
        }
        public static void RunProgram()
        {
            bool loggedIn;
            int userKey = 0;
            int adminKey = -1;

            Console.Clear();

            //looping menu, which will run when the program is started 
            do
            {
                UserInterface.DisplayWelcomeMessage();
                UserInterface.DisplayMenu(new string[] {"Logga in", "Avsluta"});
                loggedIn = false;
                string choice = UserInterface.PromptForString();
                switch (choice)
                {
                    //Logs in the user
                    case "1":
                        
                        //Logs in the user
                        LogIn(out loggedIn, out userKey); 
                        if (loggedIn)
                        {
                            UserInterface.DisplayWelcomeMessageLoggedIn(userKey);
                            
                            Utility.PressEnterToContinue();
                            Console.Clear();

                            //if login is successful, leads to CustomerMenu() 
                            CustomerMenu(userKey);
                        }
                        break;
                    
                    //Exit program
                    case "2":
                        
                        //closes the program 
                        Environment.Exit(0);
                        break;
                        
                    // Hidden admin login, which leads to AdminLogIn() and AdminMenu()
                    case "admin": 
                        AdminLogIn(out loggedIn, out adminKey);
                        if (loggedIn)
                        {
                            Admin.AdminMenu(adminKey);
                        }
                        break;
                        
                    //If neither of these options are used the defaultmsg is displayed
                    default:
                        Console.WriteLine("Felaktigt val, försök igen.");
                        Utility.PressEnterToContinue();
                        Console.Clear();
                        RunProgram();
                        break;
                }
            } while (loggedIn != true);
        }
        
        //Checks if user exist, returns userKey
        public static void LogIn(out bool loggedIn, out int userKey)
        {
            int tries = 0;
            userKey = 0; 
            loggedIn = false;

            UserInterface.DisplayWelcomeMessage();

            do
            {
                string userName = UserInterface.PromptForString("Ange ditt " +
                    "användarnamn\n\nAnvändarnamn: ").Trim();
                
                // loop through customer dictionary to search for userName
                foreach (KeyValuePair<int, Customer> item in DataBase.CustomerDict)
                {
                    if (userName == item.Value.UserName)
                    {
                        // stores userKey
                        userKey = item.Key;

                        // calls CheckPassWord function to check password
                        loggedIn = Utility.CheckPassWord(userKey); 
                                                                         
                        // if login is successful
                        if (loggedIn) { return; }
                    }

                }
                UserInterface.DisplayMessage("Användarnamnet du angett kunde " +
                    "inte hittas.\nFörsök igen");
                Utility.PressEnterToContinue();
                Utility.RemoveLines(9); 
                tries++;
            } while (tries != 3);
            
            Console.WriteLine("Ingen användare med det namnet hittades.");
            return;
        }

        public static void AdminLogIn(out bool loggedIn, out int adminKey)
        {
            int tries = 0;
            adminKey = 0;
            loggedIn = false;
            
            UserInterface.DisplayWelcomeMessage();

            do
            {
                string userName = UserInterface.PromptForString("Ange ditt " +
                    "användarnamn\n\nAnvändarnamn: ");  
                foreach (Admin item in DataBase.AdminList)
                {
                    if (userName == item.UserName)
                    {
                        adminKey = DataBase.AdminList.FindIndex(item => 
                            userName == item.UserName);

                        loggedIn = Utility.CheckAdminPassWord(adminKey);

                        if (loggedIn) { return; }
                    }
                }
                UserInterface.DisplayMessage("Användarnamnet du angett kunde " +
                    "inte hittas.\nFörsök igen");
                Utility.PressEnterToContinue();
                Utility.RemoveLines(9);
                tries++; 
            } while (tries != 3);
        }

        public static void LogOut()
        {
            // Makes the program go back to the log in menu
            RunProgram(); 
        }

        public static void CustomerMenu(int userKey)
        {
            // Looping menu  
            do 
            {
                // Creates an instance of the loggedIn user in database
                Customer obj = DataBase.CustomerDict[userKey];

                UserInterface.DisplayMessage
                    ($"{DataBase.CustomerDict[userKey].UserName}/CustomerMenu/");

                UserInterface.DisplayMenu(new string[] {"Kontoöversikt", 
                    "Överför pengar mellan egna konton", "Öppna nytt konto", 
                    "Överför pengar till annan användare", "Låna pengar", "Logga ut"});

                string choice = UserInterface.PromptForString();

                switch (choice)
                {
                    case "1":
                        // Overviews the Accounts and their respective balances
                        Console.Clear();
                        UserInterface.DisplayMessage($"{DataBase.CustomerDict[userKey].UserName}" +
                            $"/CustomerMenu/AccountOverview/");
                        obj.BankAccountOverview();
                        obj.LoanAccountOverview();
                        break;
                    case "2":
                        // Transfers a value between two accounts the user possesses
                        obj.TransferBetweenCustomerAccounts(); 
                        break;
                    case "3":
                        // Opens account for the specific user
                        obj.OpenAccount(); 
                        break;
                    case "4":
                        // Transfer money to other user in bank
                        obj.InternalMoneyTransfer(); 
                        break;
                    case "5":
                        obj.LoanMoney();
                        break;
                    case "6":
                        // Logout
                        LogOut();

                        break;

                    default:
                        Console.WriteLine("Ogiltigt val, ange ett nummer från listan.");
                        break;
                }
                Utility.PressEnterToContinue();

                // clearing console, 
                Console.Clear();

            } while (true);
        }
            

    }
}
