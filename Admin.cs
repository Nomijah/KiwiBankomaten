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


                UserInterface.CurrentMethodMagenta($"{DataBase.AdminList[adminKey].UserName}/AdminMenu/");
                UserInterface.DisplayMenu(new string[] { "Skapa ny användare", 
                    "Uppdatera växlingskurs","Visa alla användare", 
                    "Redigera ett användarkonto",
                    "Uppdatera bankkontotyper","Logga ut" });

                string choice = UserInterface.PromptForString();


                switch (choice)
                {
                    case "1":
                        Utility.RemoveLines(12);
                        admin.CreateNewUser();
                        break;
                    // Shows list of exchange rates with their values and asks
                    // if admin wants to change them.
                    case "2":
                        Utility.RemoveLines(12);
                        admin.UpdateExchangeRate();
                        break;
                    case "3":
                        Utility.RemoveLines(12);
                        UserInterface.CurrentMethodMagenta($"{admin.UserName}/AdminMenu/ViewAllUsers/");
                        admin.ViewAllUsers();
                        break;
                    case "4":
                        Utility.RemoveLines(12);
                        admin.EditUserAccount();
                        break;
                    case "5":
                        Utility.RemoveLines(12);
                        admin.SelectAccountType();
                        break;
                    case "6":
                        loggedIn = false;
                        break; 
                    // Loop repeats and switch is run again if none of the
                    // correct values are chosen.
                    default:
                        UserInterface.CurrentMethodRed("Fel input, skriv in ett korrekt värde");
                        break;
                }

                Utility.PressEnterToContinue();
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

                UserInterface.CurrentMethodMagenta($"{UserName}/AdminMenu/CreateNewUser/");
                UserInterface.CurrentMethod("Vilken sorts användare vill du skapa?");
                UserInterface.DisplayMenu(new string[] {"Customer", "Admin"});

                while (UserInterface.PromptForInt(out userType) != 1 && userType != 2)
                {
                    UserInterface.CurrentMethodRed("Fel input, skriv in ett korrekt värde");
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
                        UserInterface.CurrentMethodGreen($"Customer {userName} har " +
                            $"skapats med nyckeln {DataBase.CustomerDict.Last().Key}.");
                        break;
                    // Adds admin account to AdminList with name and password set from user input.
                    case 2: 
                        DataBase.AdminList.Add(new Admin(userName, passWord));
                        UserInterface.CurrentMethodGreen($"Admin {userName} har skapats.");
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
            UserInterface.CurrentMethod("Växelkurser: ");
            foreach (KeyValuePair<string, decimal> exchangeRates in DataBase.ExchangeRates)
            {
                Console.Write(" |");
                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"-{exchangeRates.Key}) {exchangeRates.Value}");
                Console.ForegroundColor = ConsoleColor.White;
                Utility.MoveCursorTo(85);
            }
        }
        // Method for updating a currency's exchange rate.
        public void UpdateExchangeRate()
        {
            while (true)
            {
                // Is used to make sure user answers with J/N when confirming options.
                bool answerBool;
                // The new value of the exchange rate.
                decimal newValue;
                // Used as key to get currency from database.
                string currency;
                // Loops until admin answers whether they want to update a value or not.
                
                UserInterface.CurrentMethodMagenta($"{UserName}/AdminMenu/UpdateExchangeRate/");
                ListExchangeRates();

                if (Utility.YesOrNoAdmin("Vill du ändra växlingskursen på någon valuta? J/N"))
                {
                    return;
                }
                
                Utility.RemoveLines(4);

                // If admin does want to update a value, loops until they
                // input a valid currency.
                do
                {

                    currency = UserInterface.QuestionForString("Var vänlig skriv in den valuta du " +
                        "vill ändra. SEK, USD, EUR, etc ").ToUpper();
                    if (currency == "SEK")
                    {
                        UserInterface.CurrentMethodRed("Du kan inte ändra på SEK då den är Bas valutan");
                        return;
                    }
                    if (!DataBase.ExchangeRates.ContainsKey(currency))
                    {
                        UserInterface.CurrentMethodRed("Ogiltigt värde, skriv in en valuta");
                        Utility.PressEnterToContinue();
                        Utility.RemoveLines(8);
                    }
                } while (!DataBase.ExchangeRates.ContainsKey(currency));
                
                UserInterface.CurrentMethodGreen($"Växlingskursen för {currency} - " +
                    $"{DataBase.ExchangeRates[currency]}");
                UserInterface.CurrentMethod("Var vänlig skriv in den nya " +
                    "växlingskursen för valutan");
                newValue = UserInterface.IsValueNumberCheck() ;

                // Loops until admin confirms whether or not they want to
                // apply the changes to the exchange rate.

                answerBool = Utility.YesOrNo($"Växlingskursen för {currency} kommer ändras till {newValue}.", $"Godkänner du detta? J/N");
                
                if (answerBool == false)
                {
                    Utility.RemoveLines(16);
                    DataBase.ExchangeRates[currency] = newValue;
                    UserInterface.CurrentMethodGreen($"Växlingskursen för {currency} " +
                        $"har ändrats till {newValue}");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLinesVariable(9, DataBase.ExchangeRates.Count - 1);

                }
                else if (answerBool == true)
                {
                    Utility.RemoveLines(16);
                    UserInterface.CurrentMethodRed($"Växlingskursen för {currency} " +
                        $"Kommer inte att ändras till {newValue}");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLinesVariable(9, DataBase.ExchangeRates.Count - 1);
                }
                
            }

        }
        // Method for printing out all customer accounts with their ID,
        // Username, Password and IsLocked status.
        public void ViewAllUsers()
        {
            UserInterface.CurrentMethod("Alla användarkonton i Kiwibank");
            foreach (KeyValuePair<int, Customer> customer in DataBase.CustomerDict)
            {
                Console.Write(" |");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"-ID:{customer.Value.Id}) - " +
                    $"Användarnamn:{customer.Value.UserName} - " +
                    $"Lösenord:{customer.Value.Password} - " +
                    $"Spärrat:{customer.Value.Locked}");
                Console.ForegroundColor = ConsoleColor.White;
                Utility.MoveCursorTo(85);
            }
        }
        // Method for selecting one specific user account and then giving
        // the admin a choice of what to do with the account.
        public void EditUserAccount()
        {
            bool noError;
            UserInterface.CurrentMethodMagenta($"{UserName}/AdminMenu/EditUserAccount/");

                // Prints all user accounts so admin can see which accounts
                // they can select.
                ViewAllUsers();
            do
            {
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
                        UserInterface.CurrentMethodMagenta($"{UserName}/AdminMenu/EditUserAccount/{selectedUser.UserName}");

                        UserInterface.DisplayMenu(new string[] {"Visa bankkonton", "Spärra/Avspärra",
                            "Ändra lösenord", "Återvänd till adminmenyn" });

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
                                UserInterface.CurrentMethodRed("Fel input, skriv in ett korrekt värde");
                                Utility.PressEnterToContinue();
                                break;
                        }
                        UserInterface.DisplayLogoMessage();
                    }
                }
                else
                {
                    UserInterface.CurrentMethodRed("Ogiltig användare vald, skriv in ett giltigt ID");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(8);
                    noError = false;
                }
            } while (noError == false);
        }
        // Method for locking user account if it is not locked, or unlocking
        // user account if it is locked.
        public void LockOrUnlockAccount(Customer selectedUser)
        {
            bool answer;
            // Checks if user account is locked or not, asks different question
            // depending on status.
            if (selectedUser.Locked)
            {
                answer = !Utility.YesOrNoAdmin("Kontot är spärrat", "Vill du avspärra kontot? [J/N]");
                if (answer)
                {
                    selectedUser.Locked = false;
                }
            }
            else
            {
                answer = !Utility.YesOrNoAdmin("Kontot är inte spärrat", "Vill du spärra kontot? [J/N]");
                if (answer)
                {
                    selectedUser.Locked = true;
                }
            }

        }
        // Method for changing user account's password.
        public void ChangeUserPassword(Customer selectedUser)
        {
            while (!Utility.YesOrNoAdmin($"Vill du ändra lösenordet till användaren " +
                    $"{selectedUser.Id} - {selectedUser.UserName}? J/N"))
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
                    UserInterface.CurrentMethodGreen($"Lösenordet har ändrats till {newPassWord}");
                    selectedUser.Password = newPassWord;
                    return;
                }
                else
                {
                    UserInterface.CurrentMethodRed("Du konfirmerade inte lösenordet");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(16);
                }
            }
        }
        
        // Method for selecting which account type you want to edit, bank or loan.
        public void SelectAccountType()
        {
            string answer;
            while (true)
            {
                UserInterface.CurrentMethodMagenta($"{UserName}/AdminMenu/UpdateAccountTypes");
                // Prints out all bank account types.
                UserInterface.CurrentMethod("Vilket typ av konto vill du ändra?");
                UserInterface.DisplayMenu(new string[] { "Bankkonto", "Lånekonto", "Återvänd till adminmenyn" });
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
                        UserInterface.CurrentMethodRed("Ogiltigt val. Välj alternativ 1-3.");
                        break;
                }
                Utility.PressEnterToContinue();
                Console.Clear();
                UserInterface.DisplayLogoMessage();
            }
        }
        // Method where you choose what to do with the bank account type you've chosen.
        public void UpdateAccountTypes(bool isBankAccount)
        {
            string answer;
            while (true)
            {
                UserInterface.DisplayLogoMessage();
                UserInterface.CurrentMethodMagenta($"{UserName}/AdminMenu/UpdateAccountTypes/BankAccounts");

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
                UserInterface.DisplayMenu(new string[] {"-1 Skapa ny kontotyp",
                    "-2 Uppdatera existerande kontotyp",
                    "-3 Återvänd till kontotypsmenyn"});
                answer = UserInterface.PromptForString();
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
                            UserInterface.CurrentMethodRed("Ogiltigt val. Välj alternativ 1-3.");
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
                            UserInterface.CurrentMethodRed("Ogiltigt val. Välj alternativ 1-3.");
                            break;
                    }
                }
                Utility.PressEnterToContinue();
                Console.Clear();
            }
        }
        // Method for creating new bank or loan account type.
        public void CreateNewAccountType(bool isBankAccount)
        {
            string name = "";
            decimal interest;
            bool answerBool = false;
            
            UserInterface.DisplayLogoMessage();
            UserInterface.CurrentMethodMagenta($"{UserName}/AdminMenu/UpdateAccountTypes/BankAccounts/CreateNewAccountType");

            if (isBankAccount)
            {
                UserInterface.CurrentMethod("Skriv in namnet för den nya bankkontotypen");
            }
            else
            {
                UserInterface.CurrentMethod("Skriv in namnet för den nya lånekontotypen");
            }
            
            while (name == "" || !answerBool)
            {
                UserInterface.PromptForString(out name);
                if (name == "")
                {
                    UserInterface.CurrentMethodRed($"Ett namn måste ha minst En karaktär. Ditt namn [{name}]");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(6);
                }
                else
                {
                    answerBool = !Utility.YesOrNo($"Det nya kontot får Namn [{name}]", $"Vill du godkänna detta? [J/N]");
                    if (answerBool == false)
                    {
                        Utility.RemoveLines(8);
                    }
                }
            }

            UserInterface.QuestionForDecimal("Skriv in procentenheten för räntan av det nya kontot", out interest);
            
            // Asks admin to confirm if they do want to create new bank account type
            // if no, admin is returned to previous method.
            
            if (isBankAccount)
            {
                Utility.YesOrNoAdmin($"Bankkontotypen {name} med räntan {interest}% kommer skapas", $"Godkänner du detta? J/N");
            }
            else
            {
                Utility.YesOrNoAdmin($"Lånekontotypen {name} med räntan {interest}% kommer skapas", $"Godkänner du detta? J/N");
            }

            if (isBankAccount)
            {
                DataBase.BankAccountTypes.Add(name, interest);
            }
            else
            {
                DataBase.LoanAccountTypes.Add(name, interest);
            }
            
            UserInterface.CurrentMethodGreen($"Kontotypen [{name}] har skapats.");

        }
        // Method for updating interest of current bank or loan account type.
        public void UpdateExistingAccountType(bool isBankAccount)
        {
            bool noError;
            int index;
            string key;
            decimal newValue = 0;
            bool answerBool = false;
            do
            {
                UserInterface.DisplayLogoMessage();
                UserInterface.CurrentMethodMagenta($"{UserName}/AdminMenu/UpdateAccountTypes/BankAccounts/CreateNewAccountType");

                // If user selected bank account type, print out bank account types.
                if (isBankAccount)
                {
                    DataBase.PrintAccountTypes();
                    UserInterface.CurrentMethod("Vilken Bankkontotyp vill du ändra? Välj genom att skriva in siffra.");

                }
                // If user selected loan account type, print out loan account types.
                else
                {
                    DataBase.PrintLoanAccountTypes();
                    UserInterface.CurrentMethod("Vilken Lånekontotyp vill du ändra? Välj genom att skriva in siffra.");

                }
                noError = Int32.TryParse(UserInterface.PromptForString(), out index);
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
                    UserInterface.CurrentMethodRed("Ogiltigt val. Skriv in en giltig siffra.");
                    Utility.PressEnterToContinue();
                }
            } while (!noError || !DataBase.BankAccountTypes.ContainsKey(key) && !DataBase.LoanAccountTypes.ContainsKey(key));
            // We now have the key, admin is asked to input its new interest rate.
            while (!answerBool)
            {
                do
                {
                    UserInterface.CurrentMethod($"Mata in procentenheten av det nya värdet på {key}");
                    noError = Decimal.TryParse(UserInterface.PromptForString(), out newValue);
                    if (!noError)
                    {
                        UserInterface.CurrentMethodRed("Ogiltigt val. Skriv in en giltig procentenhet");
                        Utility.PressEnterToContinue();
                        Utility.RemoveLines(8);
                    }
                } while (!noError);
                // Admin is asked to confirm whether they do want to change the interest rate
                // of the bank or loan account type.


                if (isBankAccount)
                {
                    answerBool = !Utility.YesOrNoAdmin($"Räntan på Bankkontot {key} kommer ändras till {newValue}", $"Godkänner du detta? J/N");
                    if (answerBool)
                    {
                        DataBase.BankAccountTypes[key] = newValue;
                    }
                    else
                    {
                        Utility.RemoveLines(10);
                    }
                }
                else
                {
                    answerBool = !Utility.YesOrNoAdmin($"Räntan på Lånekontot {key} kommer ändras till {newValue}", $"Godkänner du detta? J/N");
                    if (answerBool)
                    {
                        DataBase.LoanAccountTypes[key] = newValue;
                    }
                    else
                    {
                        Utility.RemoveLines(10);
                    }
                } 
            }
            UserInterface.CurrentMethodGreen($"Den nya räntan på {key} är nu {newValue}");
        }
    }
}
