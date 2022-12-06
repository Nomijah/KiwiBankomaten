﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
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
                string userType;
                string userName = "";
                string passWord = "";
                error = false;
                Console.Clear();

                UserInterface.DisplayMessage("Vilken sorts användare vill du skapa?");
                UserInterface.DisplayMenu(new string[] {"Customer", "Admin"});

                while (UserInterface.PromptForString(out userType) != "1" && userType != "2")
                {
                    UserInterface.DisplayMessage("Fel input, skriv in ett korrekt värde");
                    Utility.PressEnterToContinue();
                }

                Console.Clear();
                UserInterface.DisplayMessage("Vilket användarnamn ska den nya användaren ha?");
                userName = UserInterface.PromptForString();

                Console.Clear();
                UserInterface.DisplayMessage("Vilket lösenord ska den nya användaren ha?");
                passWord = UserInterface.PromptForString();

                Console.Clear();

                switch (userType)
                {
                    // Adds customer account to CustomerDict with name and
                    // password set from user input.
                    case "1": 
                        DataBase.CustomerDict.Add(DataBase.CustomerDict.Last().Key + 1, 
                            new Customer(userName, passWord));
                        UserInterface.DisplayMessage($"Customer {userName} har " +
                            $"skapats med nyckeln {DataBase.CustomerDict.Last().Key}.");
                        break;
                    // Adds admin account to AdminList with name and password set from user input.
                    case "2": 
                        DataBase.AdminList.Add(new Admin(userName, passWord));
                        UserInterface.DisplayMessage($"Admin {userName} har skapats.");
                        break;
                    // Loop repeats and switch is run again if none of the correct values are chosen.
                    default:
                        error = true;
                        break;
                }
            } while (error == true);
        }
        // Menu where admin can select different functions.
        public static void AdminMenu(int adminKey) 
        {
            // Used to log admin out if set to false.
            bool loggedIn = true;
            // Loop that runs so long as the admin has not chosen to log out.
            while (loggedIn == true) 
            {
                Console.Clear();
                UserInterface.DisplayAdminMessageLoggedIn(adminKey);
                UserInterface.DisplayMenu(new string[] { "Skapa ny användare", 
                    "Uppdatera växlingskurs","Visa alla användare", 
                    "Redigera ett användarkonto", "Logga ut" });
                switch (UserInterface.PromptForString())
                {
                    case "1":
                        CreateNewUser();
                        break;
                    // Shows list of exchange rates with their values and asks
                    // if admin wants to change them.
                    case "2": UpdateExchangeRate();
                        break;
                    case "3": Console.Clear(); 
                        ViewAllUsers();
                        Console.ReadKey();
                        break;
                    case "4": Console.Clear();
                        EditUserAccount();
                        break;
                    case "5": loggedIn = false;
                        Console.Clear();
                        break;
                    // Loop repeats and switch is run again if none of the
                    // correct values are chosen.
                    default:
                        Console.WriteLine("Fel input, skriv in ett korrekt värde");
                        Utility.PressEnterToContinue();
                        Utility.RemoveLines(5);
                        break;
                }
            }
            // Program.LogOut is called outside the loop and switch because
            // of possible bugs if it were to be called inside it.
            Program.LogOut(); 
        }
        // Method for printing out all currencies and their exchange rates.
        public static void ListExchangeRates()
        {
            foreach (KeyValuePair<string, decimal> exchangeRates in DataBase.ExchangeRates)
            {
                Console.WriteLine($"{exchangeRates.Key} {exchangeRates.Value}");
            }
        }
        // Method for updating a currency's exchange rate.
        public static void UpdateExchangeRate()
        {
            while (true)
            {
                // Is used to ensure the new value of the exchange rate is valid.
                bool noError;
                // Is used to make sure user answers with J/N when confirming options.
                string answer;
                // The new value of the exchange rate.
                decimal newValue;
                // Used as key to get currency from database.
                string currency;
                // Loops until admin answers whether they want to update a value or not.
                do
                {
                    Console.Clear();
                    ListExchangeRates();
                    Console.WriteLine("Vill du ändra växlingskursen på någon " +
                        "valuta? J/N");
                    answer = Console.ReadLine().ToUpper();
                } while (answer != "J" && answer != "N");
                if (answer == "J")
                {
                    // If admin does want to update a value, loops until they
                    // input a valid currency.
                    do
                    {
                        Console.Clear();
                        ListExchangeRates();
                        Console.WriteLine("Var vänlig skriv in den valuta du " +
                            "vill ändra. SEK, USD, EUR, etc ");
                        currency = Console.ReadLine().ToUpper();
                        if (!DataBase.ExchangeRates.ContainsKey(currency))
                        {
                            Console.WriteLine("Ogiltigt värde, skriv in en valuta");
                            Utility.PressEnterToContinue();
                        }
                    } while (!DataBase.ExchangeRates.ContainsKey(currency));
                    // Loops until admin inputs a valid positive number´as the new exchange rate.
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"Växlingskursen för {currency} - " +
                            $"{DataBase.ExchangeRates[currency]}");
                        Console.WriteLine("Var vänlig skriv in den nya " +
                            "växlingskursen för valutan");
                        noError = Decimal.TryParse(Console.ReadLine(), out newValue);
                        if (noError == false || newValue < 0)
                        {
                            Console.WriteLine("Ogiltigt värde, mata in en positiv siffra");
                            Utility.PressEnterToContinue();
                        }
                    } while (noError == false || newValue < 0);
                    // Loops until admin confirms whether or not they want to
                    // apply the changes to the exchange rate.
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"Växlingskursen för {currency} " +
                            $"kommer ändras till {newValue}. Godkänner du detta? J/N");
                        answer = Console.ReadLine().ToUpper();
                    } while (answer != "J" && answer != "N");
                    // If admin answers yes, exchange rate is updated.
                    if (answer == "J")
                    {
                        DataBase.ExchangeRates[currency] = newValue;
                        Console.WriteLine($"Växlingskursen för {currency} " +
                            $"har ändrats till {newValue}");
                        Utility.PressEnterToContinue();
                    }
                }
                else
                {
                    return;
                }
            }

        }
        // Method for printing out all customer accounts with their ID,
        // Username, Password and IsLocked status.
        public static void ViewAllUsers()
        {
            Console.WriteLine("---Alla användarkonton i Kiwibank---");
            foreach (KeyValuePair<int, Customer> customer in DataBase.CustomerDict)
            {
                Console.WriteLine($"ID:{customer.Value.Id} - " +
                    $"Användarnamn:{customer.Value.UserName} - " +
                    $"Lösenord:{customer.Value.Password} - " +
                    $"Spärrat:{customer.Value.Locked}");
            }
        }
        // Method for selecting one specific user account and then giving
        // the admin a choice of what to do with the account.
        public static void EditUserAccount()
        {
            bool noError;
            do
            {
                Console.Clear();
                // Prints all user accounts so admin can see which accounts
                // they can select.
                ViewAllUsers();
                Console.WriteLine("Vilken användare vill du välja? Välj " +
                    "genom att skriva in ID");
                // Admin inputs user ID here to select which account
                // they want to edit.
                noError = Int32.TryParse(Console.ReadLine(), out int userID);
                // Checks if admin input is a number and if that number exists
                // as a key in the customer dictionary.
                if (noError && DataBase.CustomerDict.ContainsKey(userID))
                {
                    // Chosen customer account is saved for access to non-static
                    // methods and easier property access.
                    Customer selectedUser = DataBase.CustomerDict[userID];
                    while (true)
                    {
                        Console.Clear();
                        Console.WriteLine($"Du har valt användaren ID:" +
                            $"{selectedUser.Id} - Användarnamn:" +
                            $"{selectedUser.UserName}" +
                            $" - Lösenord:{selectedUser.Password}" +
                            $" - Spärrad:{selectedUser.Locked}");
                        Console.WriteLine("Vad vill du göra med användaren?\n-1 " +
                            "Visa bankkonton\n-2 Spärra/Avspärra" +
                            "\n-3 Ändra lösenord\n-4 Återvänd till adminmenyn");
                        switch (Console.ReadLine())
                        {
                            // Prints all user's bank accounts.
                            case "1":
                                selectedUser.BankAccountOverview();
                                Console.ReadKey();
                                break;
                            // Lets admin lock or unlock user's account.
                            case "2":
                                LockOrUnlockAccount(selectedUser);
                                break;
                            // Lets admin change password of user's account.
                            case "3":
                                ChangeUserPassword(selectedUser);
                                break;
                            // Returns admin to the main admin menu.
                            case "4":
                                return;
                            default:
                                Console.WriteLine("Fel input, skriv in ett korrekt värde");
                                break;
                        }
                    }
                }
                else
                {
                    Console.WriteLine("Ogiltig användare vald, skriv in ett giltigt ID");
                    noError = false;
                }
            } while (noError == false);
        }
        // Method for locking user account if it is not locked, or unlocking
        // user account if it is locked.
        public static void LockOrUnlockAccount(Customer selectedUser)
        {
            string answer;
            // Checks if user account is locked or not, asks different question
            // depending on status.
            if (selectedUser.Locked)
            {
                Console.WriteLine("Kontot är spärrat");
                Console.WriteLine("Vill du avspärra kontot? J/N");
            }
            else
            {
                Console.WriteLine("Kontot är inte spärrat");
                Console.WriteLine("Vill du spärra kontot? J/N");
            }
            do
            {
                answer = Console.ReadLine().ToUpper();
                // If admin confirms that they want to lock/unlock user then we do so.
                switch (answer)
                {
                    case "J":
                        if (selectedUser.Locked)
                        {
                            selectedUser.Locked = false;
                        }
                        else
                        {
                            selectedUser.Locked = true;
                        }
                        break;
                    case "N":
                        break;
                    default:
                        Console.WriteLine("Fel input, välj antingen J/N");
                        break;
                }
            } while (answer != "J" && answer != "N");

        }
        // Method for changing user account's password.
        public static void ChangeUserPassword(Customer selectedUser)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Vill du ändra lösenordet till användaren " +
                    $"{selectedUser.Id} - {selectedUser.UserName}? J/N");
                if (Console.ReadLine().ToUpper() == "J")
                {
                    Console.Clear();
                    Console.WriteLine($"Skriv in det nya lösenordet till " +
                        $"användaren {selectedUser.UserName}");
                    string newPassWord = Console.ReadLine();
                    Console.WriteLine("Konfirmera användarens lösenord genom " +
                        "att skriva in det igen");
                    // Makes user input the password again to confirm that it
                    // is what they want.
                    if (Console.ReadLine() == newPassWord)
                    {
                        selectedUser.Password = newPassWord;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Du konfirmerade inte lösenordet");
                        Console.ReadKey();
                    }
                }
                else { return; }
            }

        }
    }
}
