using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;
using System.Diagnostics;


namespace KiwiBankomaten
{
    internal class Customer : User
    {
        private Dictionary<int, BankAccount> BankAccounts;

        // Used for creating test customers.
        public Customer(int id, string username, string password, bool locked)
        {
            Id = id;
            UserName = username;
            Password = password;
            Locked = locked;
            // Bankaccounts for testing, same for each user.
            BankAccounts = new Dictionary<int, BankAccount>()
            {
                { 1, new BankAccount("Lönekonto", 25347.34m, "SEK", 0m) },
                { 2, new BankAccount("Sparkonto", 324000m, "SEK", 2.5m) },
                { 3, new BankAccount("Utlandskonto", 74654.36m, "EUR", 1.3m) },
                { 4, new BankAccount("Företagskonto", 624.86m, "USD", 0m) }
            };
        }

        // Use this when creating customers in program.
        public Customer(string username, string password)
        {
            if (DataBase.CustomerDict == null)
            {
                Id = 1;
            }
            else
            {
                int newId = DataBase.CustomerDict.Last().Key + 1;
                Id = newId;
            }
            UserName = username;
            Password = password;
            IsAdmin = false;
            Locked = false;

            BankAccounts = new Dictionary<int, BankAccount>()
            {
                { 1, new BankAccount("Lönekonto", "SEK", 0m) }
            };
        }

        // Method for customers to open account.
        public void OpenAccount()
        {
            // Gets the interest rate of chosen account
            decimal interest = ChooseAccountType();
            // Gets the chosen name of account
            string accountName = ChooseAccountName();
            // Gets currency choice from Customer
            string currency = ChooseCurrency();
            // Gets the highest key present and adds one to get new key
            int index = BankAccounts.Keys.Max() + 1;
            // Adds the new account to customers account dictionary
            BankAccounts.Add(index, new BankAccount(accountName, currency, interest));
            InsertMoneyIntoNewAccount(interest);
        }

        // Lets the customer choose what type of account to open.
        public decimal ChooseAccountType()
        {
            Console.Clear();
            int userChoice = 0;
            // Loop until user has entered a valid choice
            while (userChoice == 0)
            {
                Console.WriteLine("Vilken typ av konto vill du öppna?");
                DataBase.PrintAccountTypes();
                Console.Write($"Välj [1 - {DataBase.BankAccountTypes.Count}]:");
                string userInput = Console.ReadLine();
                try
                {
                    userChoice = Convert.ToInt32(userInput);
                    if (userChoice < 1 ||
                        userChoice > DataBase.BankAccountTypes.Count)
                    {
                        Console.WriteLine("Felaktigt val, numret du angett " +
                            "finns inte i listan.");
                        userChoice = 0;
                        Thread.Sleep(2000);
                    }
                    else
                    {
                        string answer;
                        // Check if user is happy with the choice
                        do
                        {
                            Console.WriteLine($"Du har valt " +
                                $"{DataBase.BankAccountTypes[userChoice - 1].Item1}. " +
                                $"med ränta " +
                                $"{DataBase.BankAccountTypes[userChoice - 1].Item2}%." +
                                $" Vill du godkänna detta? [J/N]");
                            answer = Console.ReadLine().ToUpper();
                            switch (answer)
                            {
                                case "J": // If yes, do nothing
                                    break;
                                case "N": // If no, restart loop
                                    userChoice = 0;
                                    break;
                                default:
                                    Console.WriteLine("Felaktig inmatning, " +
                                        "välj [J] för ja eller [N] för nej.");
                                    break;
                            }
                            // Repeat loop until valid choice is given
                        } while (answer != "J" && answer != "N");
                    }
                }
                catch
                {
                    Console.WriteLine("Felaktig inmatning, använd endast" +
                        " siffror.");
                    Thread.Sleep(2000);
                }
                Console.Clear();
            }
            // returns the interest rate of chosen account type
            return DataBase.BankAccountTypes[userChoice - 1].Item2;
        }

        // Lets the customer choose a name for the account.
        private string ChooseAccountName()
        {
            bool notReady = true;
            string accountName;
            do
            {
                Console.WriteLine("Vilket namn vill du ge ditt konto?");
                accountName = Console.ReadLine();
                string answer;
                do
                {
                    Console.WriteLine($"Ditt konto får namnet {accountName}. " +
                        $"Vill du godkänna detta? [J/N]");
                    answer = Console.ReadLine().ToUpper();
                    switch (answer)
                    {
                        case "J":
                            notReady = false;
                            break;
                        case "N":
                            break;
                        default:
                            Console.WriteLine("Felaktig inmatning, välj [J] " +
                                "för ja eller N för nej.");
                            break;
                    }
                    // Loop until valid choice is given
                } while (answer != "J" && answer != "N");
                // Loop until user is happy with the choice
            } while (notReady);
            return accountName;
        }

        // Lets the customer choose which currency the account shall be in.
        private string ChooseCurrency()
        {
            Console.Clear();
            Console.WriteLine("Vilken valuta vill du använda till ditt konto?" +
                "\nTillgängliga valutor:");

            DataBase.PrintCurrencies();

            Console.Write("Ange valuta: ");
            string currency = Console.ReadLine().ToUpper();
            // Check if user input is correct, if not ask again
            while (!DataBase.ExchangeRates.ContainsKey(currency))
            {
                Console.Clear();
                Console.WriteLine("Valutan du angett finns inte i systemet," +
                    "vänligen välj en valuta från listan.");
                DataBase.PrintCurrencies();
                Console.Write("Ange valuta: ");
                currency = Console.ReadLine().ToUpper();
            }
            return currency;
        }
        public void InsertMoneyIntoNewAccount(decimal interest)
        {
            bool noError;
            decimal insertAmount;
            Console.Clear();
            Console.WriteLine($"Vill du sätta in {BankAccounts[BankAccounts.Keys.Max()].Currency} i ditt nya konto? J/N");
            string answer;
            do
            {
                answer = Console.ReadLine().ToUpper();
                switch (answer)
                {
                    case "J":
                        break;
                    case "N":
                        return;
                    default:
                        Console.WriteLine("Felaktig inmatning, välj [J] " +
                            "för ja eller N för nej.");
                        break;
                }
            } while (answer != "J" && answer != "N");
            do
            {
                noError = true;
                Console.WriteLine("Skriv in mängden pengar du vill sätta in");
                if (decimal.TryParse(Console.ReadLine(), out insertAmount) && insertAmount >= 0)
                {
                    BankAccounts[BankAccounts.Keys.Max()].Amount += insertAmount;
                }
                else
                {
                    Console.WriteLine("Det där är inte ett giltigt värde");
                    noError = false;
                }
            } while (noError == false);
            ViewInterestSavingsOfNewAccount(interest, insertAmount);
        }
        public void ViewInterestSavingsOfNewAccount(decimal interest, decimal insertAmount)
        {
            decimal interestAmount = insertAmount * interest / 100;
            Console.WriteLine("Såhär mycket ränta kommer du tjäna med den angivna summan: ");
            Console.WriteLine("1 år : " + Math.Round(interestAmount, 2));
            for (int i = 0; i < 4; i++)
            {
                interestAmount += (insertAmount + interestAmount) * interest / 100;
            }
            Console.WriteLine("5 år : " + Math.Round(interestAmount, 2));
            for (int i = 0; i < 5; i++)
            {
                interestAmount += (insertAmount + interestAmount) * interest / 100;
            }
            Console.WriteLine("10 år : " + Math.Round(interestAmount, 2));
        }

        // Prints out users accounts.
        public void AccountOverview()
        {
        
            // Print out each account with key, number, name, value and currency
            foreach (KeyValuePair<int, BankAccount> account in BankAccounts)
            {
               Console.WriteLine($"{account.Key}. {account.Value.AccountNumber} " +
                   $"{account.Value.AccountName}: {Math.Round(account.Value.Amount,2)} " +
                   $"{account.Value.Currency}");
            }
        
        }
 
        // Shows the Customer the Accounts that was involved in the transaction
        public void AccountOverview(int fromWhichAccount, int toWhichAccount)
        {
            Console.WriteLine("Money was sent from : ");
            Console.WriteLine($"KontoNamn : {BankAccounts[fromWhichAccount].AccountName} - KontoSaldo : {Math.Round(BankAccounts[fromWhichAccount].Amount, 2)} {BankAccounts[fromWhichAccount].Currency}\n");
            Console.WriteLine("Money was sent to : ");
            Console.WriteLine($"KontoNamn : {BankAccounts[toWhichAccount].AccountName} - KontoSaldo : {Math.Round(BankAccounts[toWhichAccount].Amount, 2)} {BankAccounts[toWhichAccount].Currency}");
        }

        //Initalizes the transfer between accounts
        public void TransferBetweenCustomerAccounts()
        {
            decimal amountMoney;
            int transferFromWhichAccount;
            int transferToWhichAccount;

            Console.Clear();
            // Shows the Customer their Accounts and the balances in said Accounts
            AccountOverview(); 

            Console.WriteLine("Från vilket konto vill du föra över pengarna?");
            Utility.IsValueNumberCheck(out transferFromWhichAccount, BankAccounts.Count);

            bool correctAmount = false;
            do
            {
                Console.WriteLine("Hur mycket pengar vill du föra över?");
                Utility.IsValueNumberCheck(out amountMoney);
                if (!CheckAccountValue(transferFromWhichAccount, amountMoney))
                {
                    Console.WriteLine("Summan du har angett finns inte på kontot, " +
                        "försök igen.");
                }
                else
                {
                    correctAmount = true;
                }
            } while (!correctAmount);

            Console.WriteLine("Vilket konto vill du föra över pengarna till?");
            // Gets User input and Checks if it's Valid
            Utility.IsValueNumberCheck(out transferToWhichAccount, BankAccounts.Count); 

            TransferMoney(BankAccounts[transferToWhichAccount].AccountNumber,
                    BankAccounts[transferFromWhichAccount].AccountNumber, amountMoney);
            AccountOverview(transferFromWhichAccount, transferToWhichAccount);

        }
            
        // Method to check if two internal accounts use the same currency.
        private bool CurrencyCheck(int toAccountNum, int fromAccountNum)
        {
            string toCurrency = "";
            string fromCurrency = "";
            foreach (Customer c in DataBase.CustomerDict.Values)
            {
                foreach (BankAccount b in c.BankAccounts.Values)
                {
                    if (toAccountNum == b.AccountNumber)
                    {
                        toCurrency = b.Currency;
                    }
                    else if (fromAccountNum == b.AccountNumber)
                    {
                        fromCurrency = b.Currency;
                    }
                }
            }
            if (toCurrency == fromCurrency)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Method for transferring money with currency exchange.
        public void TransferMoney(int toAccountNum,
            int fromAccountNum, decimal amountMoney)
        {
            decimal toRate = 1;
            decimal fromRate = 1;
            string toCurrency = "";
            string fromCurrency = "";
            // Check which currency the accounts are in
            foreach (Customer c in DataBase.CustomerDict.Values)
            {
                foreach (BankAccount b in c.BankAccounts.Values)
                {
                    if (toAccountNum == b.AccountNumber)
                    {
                        toCurrency = b.Currency;
                    }
                    else if (fromAccountNum == b.AccountNumber)
                    {
                        fromCurrency = b.Currency;
                    }
                }
            }
            foreach (KeyValuePair<string, decimal> item in DataBase.ExchangeRates)
            {
                // When match is found, save conversion rate
                if (item.Key == toCurrency)
                {
                    toRate = item.Value;
                }
                if (item.Key == fromCurrency)
                {
                    fromRate = item.Value;
                }
            }

            foreach (Customer c in DataBase.CustomerDict.Values)
            {
                foreach (BankAccount b in c.BankAccounts.Values)
                {
                    if (toAccountNum == b.AccountNumber)
                    {
                        // Add converted value to target account.
                        b.Amount += (amountMoney / toRate) * fromRate;
                    }
                    else if (fromAccountNum == b.AccountNumber)
                    {
                        // Withdraw from the source account.
                        b.Amount -= amountMoney;
                    }
                }
            }   
            Console.WriteLine("Överföringen lyckades.");
            Utility.PressEnterToContinue();
        }
        public void InternalMoneyTransfer()
        {
            decimal amountMoney;
            int transferFromWhichAccount;
            decimal transferToWhichAccount;

            Console.Clear();
            AccountOverview();

            Console.WriteLine("Från vilket konto vill du föra över pengarna?");
            Utility.IsValueNumberCheck(out transferFromWhichAccount, BankAccounts.Count);

            bool correctAmount = false;
            do
            {
                Console.WriteLine("Hur mycket pengar vill du föra över?");
                Utility.IsValueNumberCheck(out amountMoney);
                if (!CheckAccountValue(transferFromWhichAccount, amountMoney))
                {
                    Console.WriteLine("Summan du har angett finns inte på kontot, " +
                        "försök igen.");
                }
                else
                {
                    correctAmount = true;
                }
            } while (!correctAmount);

            bool correctAccountNumber = false;
            do
            {
                Console.WriteLine("Skriv det 8-siffriga kontonummer du vill föra över pengar till:");
                Utility.IsValueNumberCheck(out transferToWhichAccount);
                correctAccountNumber = CheckIfAccountExists((int)transferToWhichAccount);
                if (!correctAccountNumber)
                {
                    if (!Utility.ContinueOrAbort())
                    {
                        return;
                    }
                }
            } while (!correctAccountNumber);

                TransferMoney((int)transferToWhichAccount,
                BankAccounts[transferFromWhichAccount].AccountNumber, amountMoney);

        }

        // Checks if account exists in registry.
        public bool CheckIfAccountExists(int accountNum)
        {
            // Check every user in database.
            foreach (Customer customer in DataBase.CustomerDict.Values)
            {
                // Check each account for match.
                foreach (BankAccount acc in customer.BankAccounts.Values)
                {
                    // If account is found and return true.
                    if (acc.AccountNumber == accountNum)
                    {
                        return true;
                    }
                }
            }
            //If account is not found, print error message and return false.
            Console.WriteLine("Kontot du har angett finns inte i banken.");
            return false;
        }

        // Checks if account holds the chosen amount
        public bool CheckAccountValue(int accountNum, decimal amountMoney)
        {
            if (amountMoney > BankAccounts[accountNum].Amount)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}