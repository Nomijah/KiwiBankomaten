using SlackAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace KiwiBankomaten
{
    internal class Admin : User
    {
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
        }
        // Menu where admin can select different functions.
        public static void AdminMenu(int adminKey)
        {
            // Used to log admin out if set to false.
            bool loggedIn = true;

            Utility.RemoveLines(16);

            // Loop that runs so long as the admin has not chosen to log out.
            while (loggedIn == true)
            {

                Admin admin = DataBase.AdminList[adminKey];


                UserInterface.CurrentMethod($"{DataBase.AdminList[adminKey].UserName}/AdminMenu/");
                UserInterface.DisplayMenu(new string[] { "Skapa ny användare", 
                    "Uppdatera växlingskurs","Visa alla användare", 
                    "Redigera ett användarkonto",
                    "Uppdatera bankkontotyper","Logga ut" });

                string choice = UserInterface.PromptForString();

                Utility.RemoveLines(12);

                switch (choice)
                {
                    case "1":
                        admin.CreateNewUser();
                        break;
                    // Shows list of exchange rates with their values and asks
                    // if admin wants to change them.
                    case "2": 
                        admin.UpdateExchangeRate();
                        break;
                    case "3":
                        UserInterface.CurrentMethod($"{admin.UserName}/AdminMenu/ViewAllUsers/");
                        admin.ViewAllUsers();
                        break;
                    case "4": 
                        admin.EditUserAccount();
                        break;
                    case "5": 
                        admin.UpdateAccountTypes();
                        break;
                    case "6":
                        loggedIn = false;
                        break;
                    // Loop repeats and switch is run again if none of the
                    // correct values are chosen.
                    default:
                        UserInterface.CurrentMethod("Fel input, skriv in ett korrekt värde");
                        break;
                }

                Utility.PressEnterToContinue();
                Console.Clear();
                UserInterface.DisplayLogoMessage();
            }
            // Program.LogOut is called outside the loop and switch because
            // of possible bugs if it were to be called inside it.
            Program.LogOut();
        }
        // Admin method for creating new users.
        public void CreateNewUser() 
        {
            // Used in the do-while loop to repeat if any input errors are detected.
            bool error; 
            do
            {
                int userType;
                string userName = "";
                string passWord = "";
                error = false;

                UserInterface.CurrentMethod($"{UserName}/AdminMenu/CreateNewUser/");
                UserInterface.CurrentMethod("Vilken sorts användare vill du skapa?");
                UserInterface.DisplayMenu(new string[] {"Customer", "Admin"});

                while (UserInterface.PromptForInt(out userType) != 1 && userType != 2)
                {
                    UserInterface.DisplayMessage("Fel input, skriv in ett korrekt värde");
                    Utility.PressEnterToContinue();
                }

                userName = UserInterface.QuestionForString("Vilket användarnamn ska den nya användaren ha?");

                passWord = UserInterface.QuestionForString("Vilket lösenord ska den nya användaren ha?");

                switch (userType)
                {
                    // Adds customer account to CustomerDict with name and
                    // password set from user input.
                    case 1: 
                        DataBase.CustomerDict.Add(DataBase.CustomerDict.Last().Key + 1, 
                            new Customer(userName, passWord));
                        UserInterface.CurrentMethod($"Customer {userName} har " +
                            $"skapats med nyckeln {DataBase.CustomerDict.Last().Key}.");
                        break;
                    // Adds admin account to AdminList with name and password set from user input.
                    case 2: 
                        DataBase.AdminList.Add(new Admin(userName, passWord));
                        UserInterface.CurrentMethod($"Admin {userName} har skapats.");
                        break;
                    // Loop repeats and switch is run again if none of the correct values are chosen.
                    default:
                        error = true;
                        break;
                }
            } while (error == true);

        }
        // Method for printing out all currencies and their exchange rates.
        public void ListExchangeRates()
        {
            UserInterface.CurrentMethod("Växelkurser");
            foreach (KeyValuePair<string, decimal> exchangeRates in DataBase.ExchangeRates)
            {
                Console.Write($" |-{exchangeRates.Key}) {exchangeRates.Value}");
                Utility.MoveCursorTo(85);
            }
        }
        // Method for updating a currency's exchange rate.
        public void UpdateExchangeRate()
        {
            while (true)
            {
                // Is used to make sure user answers with J/N when confirming options.
                string answer;
                // The new value of the exchange rate.
                decimal newValue;
                // Used as key to get currency from database.
                string currency;
                // Loops until admin answers whether they want to update a value or not.
                
                UserInterface.CurrentMethod($"{UserName}/AdminMenu/UpdateExchangeRate/");
                ListExchangeRates();
                do
                {
                    answer = UserInterface.QuestionForString("Vill du ändra växlingskursen på någon " +
                        "valuta? J/N").ToUpper();
                    if (answer != "J" && answer != "N")
                    {
                        UserInterface.CurrentMethod("Men seriöst kan du inte följa dem mest grundläggande instruktioner?");
                        Utility.PressEnterToContinue();
                        Utility.RemoveLines(8);
                    }
                } while (answer != "J" && answer != "N");
                
                    Utility.RemoveLines(4);

                if (answer == "J")
                {
                    // If admin does want to update a value, loops until they
                    // input a valid currency.
                    do
                    {

                        currency = UserInterface.QuestionForString("Var vänlig skriv in den valuta du " +
                            "vill ändra. SEK, USD, EUR, etc ").ToUpper();
                        if (!DataBase.ExchangeRates.ContainsKey(currency))
                        {
                            UserInterface.CurrentMethod("Ogiltigt värde, skriv in en valuta");
                            Utility.PressEnterToContinue();
                            Utility.RemoveLines(8);
                        }
                    } while (!DataBase.ExchangeRates.ContainsKey(currency));
                    
                    UserInterface.CurrentMethod($"Växlingskursen för {currency} - " +
                        $"{DataBase.ExchangeRates[currency]}");
                    UserInterface.CurrentMethod("Var vänlig skriv in den nya " +
                        "växlingskursen för valutan");
                    newValue = UserInterface.IsValueNumberCheck() ;
                    
                    // Loops until admin confirms whether or not they want to
                    // apply the changes to the exchange rate.
                    do
                    {
                        answer = UserInterface.QuestionForString($"Växlingskursen för {currency} " +
                            $"kommer ändras till {newValue}. Godkänner du detta? J/N").ToUpper();
                        if (answer != "J" && answer != "N")
                        {
                            UserInterface.CurrentMethod("Men seriöst kan du inte följa dem mest grundläggande instruktioner?");
                            Utility.PressEnterToContinue();
                            Utility.RemoveLines(8);
                        }
                        else if (answer == "J")
                        {
                            Utility.RemoveLines(14);
                            DataBase.ExchangeRates[currency] = newValue;
                            UserInterface.CurrentMethod($"Växlingskursen för {currency} " +
                                $"har ändrats till {newValue}");
                            Utility.PressEnterToContinue();
                            Utility.RemoveLinesVariable(9, DataBase.ExchangeRates.Count - 1);

                        }
                        else if (answer == "N")
                        {
                            Utility.RemoveLines(14);
                            UserInterface.CurrentMethod($"Växlingskursen för {currency} " +
                                $"Kommer inte att ändras till {newValue}");
                            Utility.PressEnterToContinue();
                            Utility.RemoveLinesVariable(9, DataBase.ExchangeRates.Count - 1);
                        }
                    } while (answer != "J" && answer != "N");
                }
                else
                {
                    return;
                }
            }

        }
        // Method for printing out all customer accounts with their ID,
        // Username, Password and IsLocked status.
        public void ViewAllUsers()
        {
            UserInterface.CurrentMethod("---Alla användarkonton i Kiwibank---");
            foreach (KeyValuePair<int, Customer> customer in DataBase.CustomerDict)
            {
                Console.Write($" |-ID:{customer.Value.Id}) - " +
                    $"Användarnamn:{customer.Value.UserName} - " +
                    $"Lösenord:{customer.Value.Password} - " +
                    $"Spärrat:{customer.Value.Locked}");
                Utility.MoveCursorTo(85);
            }
        }
        // Method for selecting one specific user account and then giving
        // the admin a choice of what to do with the account.
        public void EditUserAccount()
        {
            bool noError;
            UserInterface.CurrentMethod($"{UserName}/AdminMenu/EditUserAccount/");

            do
            {
                // Prints all user accounts so admin can see which accounts
                // they can select.
                ViewAllUsers();
                UserInterface.CurrentMethod("Vilken användare vill du välja? Välj " +
                    "genom att skriva in ID");
                // Admin inputs user ID here to select which account
                // they want to edit.
                noError = Int32.TryParse(UserInterface.PromptForString(), out int userID);
                // Checks if admin input is a number and if that number exists
                // as a key in the customer dictionary.
                if (noError && DataBase.CustomerDict.ContainsKey(userID))
                {
                    // Chosen customer account is saved for access to non-static
                    // methods and easier property access.
                    Customer selectedUser = DataBase.CustomerDict[userID];

                    Utility.RemoveLinesVariable(9, DataBase.CustomerDict.Count - 1);
                    while (true)
                    {
                        UserInterface.CurrentMethod($"{UserName}/AdminMenu/EditUserAccount/{selectedUser.UserName}");

                        UserInterface.DisplayMenu(new string[] {"-1 Visa bankkonton", "-2 Spärra/Avspärra",
                            "-3 Ändra lösenord", "-4 Återvänd till adminmenyn" });

                        switch (UserInterface.PromptForString())
                        {
                            // Prints all user's bank accounts.
                            case "1":
                                selectedUser.BankAccountOverview();
                                selectedUser.LoanAccountOverview();
                                Utility.PressEnterToContinue();
                                break;
                            // Lets admin lock or unlock user's account.
                            case "2":
                                LockOrUnlockAccount(selectedUser);
                                Utility.PressEnterToContinue();
                                break;
                            // Lets admin change password of user's account.
                            case "3":
                                ChangeUserPassword(selectedUser);
                                Utility.PressEnterToContinue();
                                break;
                            // Returns admin to the main admin menu.
                            case "4":
                                return;
                            default:
                                Console.WriteLine("Fel input, skriv in ett korrekt värde");
                                break;
                        }
                        UserInterface.DisplayLogoMessage();
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
        public void LockOrUnlockAccount(Customer selectedUser)
        {
            string answer;
            // Checks if user account is locked or not, asks different question
            // depending on status.
            if (selectedUser.Locked)
            {
                UserInterface.CurrentMethod("Kontot är spärrat");
                UserInterface.CurrentMethod("Vill du avspärra kontot? J/N");
            }
            else
            {
                UserInterface.CurrentMethod("Kontot är inte spärrat");
                UserInterface.CurrentMethod("Vill du spärra kontot? J/N");
            }
            do
            {
                answer = UserInterface.PromptForString().ToUpper();
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
                        UserInterface.CurrentMethod("Fel input, välj antingen J/N");
                        break;
                }
            } while (answer != "J" && answer != "N");

        }
        // Method for changing user account's password.
        public void ChangeUserPassword(Customer selectedUser)
        {
            while (true)
            {
                UserInterface.CurrentMethod($"Vill du ändra lösenordet till användaren " +
                    $"{selectedUser.Id} - {selectedUser.UserName}? J/N");
                if (UserInterface.PromptForString().ToUpper() == "J")
                {
                    UserInterface.CurrentMethod($"Skriv in det nya lösenordet till " +
                        $"användaren {selectedUser.UserName}");
                    string newPassWord = UserInterface.PromptForString();
                    UserInterface.CurrentMethod("Konfirmera användarens lösenord genom " +
                        "att skriva in det igen");
                    // Makes user input the password again to confirm that it
                    // is what they want.
                    if (UserInterface.PromptForString() == newPassWord)
                    {
                        selectedUser.Password = newPassWord;
                        return;
                    }
                    else
                    {
                        UserInterface.CurrentMethod("Du konfirmerade inte lösenordet");
                        Utility.PressEnterToContinue();
                        Utility.RemoveLines(16);
                    }
                }
                else { return; }
            }

        }
        public void ViewAccountTypes(int selection)
        // Method for selecting which account type you want to edit, bank or loan.
        public static void SelectAccountType()
        {
            string answer;
            while (true)
            {
                // Prints out all bank account types.
                UserInterface.CurrentMethod("Olika typer av bankkonto, namn och ränta:");
                ViewAccountTypes(1);
                UserInterface.CurrentMethod("Olika typer av lånekonto, namn och ränta:");
                ViewAccountTypes(2);
                UserInterface.CurrentMethod("Vilket typ av konto vill du ändra?");
                UserInterface.DisplayMenu(new string[] { "-1 Bankkonto", "-2 Lånekonto", "-3 Återvänd till adminmenyn" });
                answer = UserInterface.PromptForString();
                switch (answer)
                {
                    case "1":
                        // Runs method for choosing what to do with the bank account types.
                        // The true means the user has selected bank account type.
                        UpdateAccountTypes(true);
                        break;
                    case "2":
                        // Runs method for choosing what to do with the loan account types.
                        // The false means the user has selected loan account type.
                        UpdateAccountTypes(false);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Ogiltigt val. Välj alternativ 1-3.");
                        Utility.PressEnterToContinue();
                        break;
                }
            }
        }
        // Method where you choose what to do with the bank account type you've chosen.
        public static void UpdateAccountTypes(bool isBankAccount)
        {
            string answer;
            while (true)
            {
                Console.Clear();
                // Checks if user selected bank account type in previous menu.
                if (isBankAccount)
                {
                    // Prints out all bank account types.
                    DataBase.ViewAccountTypes(1);
                }
                else
                {
                    // Prints out all loan account types.
                    DataBase.ViewAccountTypes(2);
                }
                Console.WriteLine("---------------------------");
                Console.WriteLine("Vad vill du göra?");
                Console.WriteLine("-1 Skapa ny kontotyp\n" +
                    "-2 Uppdatera existerande kontotyp\n" +
                    "-3 Återvänd till kontotypsmenyn");
                answer = Console.ReadLine();
                // Checks if user selected bank account type in previous method.
                // If yes, we run a switch for creating new or editing current bank account types.
                if (isBankAccount)
                {
                    switch (answer)
                    {
                        case "1":
                            CreateNewAccountType(true);
                            break;
                        case "2":
                            UpdateExistingAccountType(true);
                            break;
                        case "3":
                            return;
                        default:
                            Console.WriteLine("Ogiltigt val. Välj alternativ 1-3.");
                            Utility.PressEnterToContinue();
                            break;
                    }
                }
                // If user didn't select bank account types, we instead
                // create new or edit current loan account types.
                else
                {
                    switch (answer)
                    {
                        case "1":
                            CreateNewAccountType(false);
                            break;
                        case "2":
                            UpdateExistingAccountType(false);
                            break;
                        case "3":
                            return;
                        default:
                            Console.WriteLine("Ogiltigt val. Välj alternativ 1-3.");
                            Utility.PressEnterToContinue();
                            break;
                    }
                }
            }
        }
        // Method for creating new bank or loan account type.
        public static void CreateNewAccountType(bool isBankAccount)
        {
            bool noError;
            string name;
            decimal interest;
            string answer;
            do
            {
                Console.Clear();
                if (isBankAccount)
                {
                    Console.WriteLine("Skriv in namnet för den nya bankkontotypen");
                }
                else
                {
                    Console.WriteLine("Skriv in namnet för den nya lånekontotypen");
                }
                
                name = Console.ReadLine();
                Console.WriteLine("Skriv in procentenheten för räntan av det nya kontot");
                noError = Decimal.TryParse(Console.ReadLine(), out interest);
                if (!noError)
                {
                    Console.WriteLine("Ogiltigt värde, skriv in en procentenhet för räntan.");
                    Utility.PressEnterToContinue();
                }
            } while (!noError);

            // Asks admin to confirm if they do want to create new bank account type
            // if no, admin is returned to previous method.
            do
            {
                Console.Clear();
                if (isBankAccount)
                {
                    Console.WriteLine($"Bankkontotypen {name} med räntan {interest}% kommer skapas. " +
                        $"Godkänner du detta? J/N");
                }
                else
                {
                    Console.WriteLine($"Lånekontotypen {name} med räntan {interest}% kommer skapas. " +
                        $"Godkänner du detta? J/N");
                }
                answer = Console.ReadLine().ToUpper();
                // If admin does confirm they want to create a new account type,
                // new bank or loan account type is created.
                switch (answer)
                {
                    case "J":
                        if (isBankAccount)
                        {
                            DataBase.BankAccountTypes.Add(name, interest);
                        }
                        else
                        {
                            DataBase.LoanAccountTypes.Add(name, interest);
                        }
                        break;
                    case "N":
                        return;
                    default:
                        Console.WriteLine("Fel input, välj antingen J/N");
                        Utility.PressEnterToContinue();
                        break;
                }
            } while (answer != "J" && answer != "N");
            Console.WriteLine($"Kontotypen {name} har skapats.");
            Utility.PressEnterToContinue();

        }
        // Method for updating interest of current bank or loan account type.
        public static void UpdateExistingAccountType(bool isBankAccount)
        {
            bool noError;
            int index;
            string key;
            decimal newValue;
            string answer;
            do
            {
                Console.Clear();
                // If user selected bank account type, print out bank account types.
                if (isBankAccount)
                {
                    DataBase.PrintAccountTypes();
                    
                }
                // If user selected loan account type, print out loan account types.
                else
                {
                    DataBase.PrintLoanAccountTypes();
                }
                Console.WriteLine("Vilken kontotyp vill du ändra? Välj genom att skriva in siffra.");
                noError = Int32.TryParse(Console.ReadLine(), out index);
                index -= 1;
                // Gets key from bank or loan account type using its index.
                if (isBankAccount)
                {
                    key = DataBase.GetKeyFromBankTypeIndex(index);
                }
                else
                {
                    key = DataBase.GetKeyFromLoanTypeIndex(index);
                }
                if (!noError || !DataBase.BankAccountTypes.ContainsKey(key) && !DataBase.LoanAccountTypes.ContainsKey(key))
                {
                    Console.WriteLine("Ogiltigt val. Skriv in en giltig siffra.");
                    Utility.PressEnterToContinue();
                }
            } while (!noError || !DataBase.BankAccountTypes.ContainsKey(key) && !DataBase.LoanAccountTypes.ContainsKey(key));
            // We now have the key, admin is asked to input its new interest rate.
            do
            {
                Console.Clear();
                Console.WriteLine($"Mata in procentenheten av det nya värdet på {key}");
                noError = Decimal.TryParse(Console.ReadLine(), out newValue);
                if (!noError)
                {
                    Console.WriteLine("Ogiltigt val. Skriv in en giltig procentenhet.");
                    Utility.PressEnterToContinue();
                }
            } while (!noError);
            // Admin is asked to confirm whether they do want to change the interest rate
            // of the bank or loan account type.
            do
            {
                Console.Clear();
                Console.WriteLine($"Räntan på kontotypen {key} kommer ändras till {newValue}. " +
                    $"Godkänner du detta? J/N");
                answer = Console.ReadLine().ToUpper();
                switch (answer)
                {
                    case "J":
                        if (isBankAccount)
                        {
                            DataBase.BankAccountTypes[key] = newValue;
                        }
                        else
                        {
                            DataBase.LoanAccountTypes[key] = newValue;
                        }
                        break;
                    case "N":
                        return;
                    default:
                        Console.WriteLine("Fel input, välj antingen J/N");
                        Utility.PressEnterToContinue();
                        break;
                }
            } while (answer != "J" && answer != "N");

            Console.WriteLine($"Den nya räntan på {key} är nu {newValue}");
            Utility.PressEnterToContinue();
        }
    }
}
