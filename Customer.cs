﻿using System;
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
            // Asks user if they want to put money into new account, if yes money is created in account.
            InsertMoneyIntoNewAccount(interest);
        }

        // Lets the customer choose what type of account to open.
        public decimal ChooseAccountType()
        {
            int userChoice = 0;
            Console.Clear();
            UserInterface.DisplayMessage("Vilken typ av konto vill du öppna?");
            UserInterface.DisplayMessage("Tillgängliga Kontotyper");
            DataBase.PrintAccountTypes();
            do
            {
                userChoice = 0;
                while ((!int.TryParse(UserInterface.PromptForString(), out userChoice)) 
                    || userChoice < 1 || userChoice > DataBase.BankAccountTypes.Count)
                {
                    UserInterface.DisplayMessage("Felaktigt val\n" +
                        "Numret du angett finns inte i listan.");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(7);
                }
            } while (Utility.YesOrNo("Kontotyp: ", DataBase.BankAccountTypes[userChoice - 1].Item1));
            
            // returns the interest rate of chosen account type
            return DataBase.BankAccountTypes[userChoice - 1].Item2;
        }

        // Lets the customer choose a name for the account.
        private string ChooseAccountName()
        {
            string accountName;

            Console.Clear();
            UserInterface.DisplayMessage("Vilket namn vill du ge ditt konto?");
            do
            {
                accountName = UserInterface.PromptForString();

                // Loop until user is happy with the choice
            } while (Utility.YesOrNo("namn", accountName));
                
            Console.Clear();
            return accountName;
        }

        // Lets the customer choose which currency the account shall be in.
        private string ChooseCurrency()
        {
            string currency;
            UserInterface.DisplayMessage("Vilken valuta vill du använda till ditt konto?");
            UserInterface.DisplayMessage("Tillgängliga valutor");
            DataBase.PrintCurrencies();
            do
            {

                // Check if user input is correct, if not ask again
                while (!DataBase.ExchangeRates.ContainsKey(UserInterface.PromptForString(out currency).ToUpper()))
                {
                    UserInterface.DisplayMessage("Valutan du angett finns inte i systemet\n" +
                        "Vänligen välj en valuta från listan");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(7);
                } 
            } while (Utility.YesOrNo("Valuta", currency.ToUpper()));

            return currency.ToUpper();
        }

        // Method for adding money into newly created account.
        public void InsertMoneyIntoNewAccount(decimal interest)
        {
            string answer;
            // Used to ensure money amount is valid, has to be a positive number.
            bool noError;
            // Amount of money to be inserted into new account.
            decimal insertAmount;

            Console.Clear();
            UserInterface.DisplayMessage($"Vill du sätta in {BankAccounts[BankAccounts.Keys.Max()].Currency} i ditt nya konto? J/N");
            // Will only proceed if user selects J or N.
            do
            {
                answer = UserInterface.PromptForString().ToUpper();
                switch (answer)
                {
                    case "J":
                        break;
                    case "N":
                        return;
                    default:
                        Console.WriteLine("Felaktig inmatning, välj [J] " +
                            "för ja eller N för nej.");
                        Utility.PressEnterToContinue();
                        Utility.RemoveLines(4);
                        break;
                }
            } while (answer != "J" && answer != "N");
            // Loop runs until user has chosen a valid amount of money to put into account.
            do
            {
                Console.Clear();
                noError = true;
                UserInterface.DisplayMessage("Skriv in mängden pengar du vill sätta in");
                // Checks if amount to be inserted is a number and checks if it is a positive number.
                if (decimal.TryParse(UserInterface.PromptForString(), out insertAmount) && insertAmount >= 0)
                {
                    // Adds money into newly created account.
                    BankAccounts[BankAccounts.Keys.Max()].Amount += insertAmount;
                }
                else
                {
                    UserInterface.DisplayMessage("Det där är inte ett giltigt värde");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(6);
                    noError = false;
                }
            } while (noError == false);
            ViewInterestSavingsOfNewAccount(interest, insertAmount);
        }
        
        // Method for printing the amount of money the account's interest will earn them over different amounts of time
        // with the money they've just inserted.
        public void ViewInterestSavingsOfNewAccount(decimal interest, decimal insertAmount)
        {
            decimal interestAmount = insertAmount * interest / 100;
            UserInterface.DisplayMessage($"Såhär mycket ränta kommer du tjäna med {insertAmount} {BankAccounts.Last().Value.Currency}");
            UserInterface.DisplayMessage("1 år : " + Math.Round(interestAmount, 2));
            // Calculates amount of money account will earn in interest in 5 years.
            for (int i = 0; i < 4; i++)
            {
                interestAmount += (insertAmount + interestAmount) * interest / 100;
            }
            Console.WriteLine("5 år : " + Math.Round(interestAmount, 2));

            // Calculates amount of money account will earn in interest in 10 years.
            for (int i = 0; i < 5; i++)
            {
                interestAmount += (insertAmount + interestAmount) * interest / 100;
            }
            Console.WriteLine("10 år : " + Math.Round(interestAmount, 2));

        }

        // Prints out users accounts.
        public void AccountOverview()
        {
            Console.WriteLine("---------------------------------------------------");
            // Print out each account with key, number, name, value and currency
            foreach (KeyValuePair<int, BankAccount> account in BankAccounts)
            {
                Console.WriteLine($"-{account.Key}). {account.Value.AccountNumber} " +
                    $"{account.Value.AccountName}: {Utility.AmountDecimal(account.Value.Amount)} " +
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
            decimal amountMoney = 0;
            int transferFrom = 0;
            int transferTo = 0;

            DisplayTransferBetweenCustomerAccounts(transferFrom, amountMoney, transferTo);

            Console.WriteLine("Från vilket konto vill du föra över pengarna?");
            transferFrom = UserInterface.IsValueNumberCheck(BankAccounts.Count);

            DisplayTransferBetweenCustomerAccounts(transferFrom, amountMoney, transferTo);


            bool correctAmount = false;
            do
            {
                Console.WriteLine("Hur mycket pengar vill du föra över?");
                amountMoney = UserInterface.IsValueNumberCheck();
                if (!CheckAccountValue(transferFrom, amountMoney))
                {
                    Console.WriteLine("Summan du har angett finns inte på kontot, " +
                        "försök igen.");
                }
                else
                {
                    correctAmount = true;
                }
            } while (!correctAmount);

            DisplayTransferBetweenCustomerAccounts(transferFrom, amountMoney, transferTo);

            Console.WriteLine("Vilket konto vill du föra över pengarna till?");
            // Gets User input and Checks if it's Valid
            transferTo = UserInterface.IsValueNumberCheck(BankAccounts.Count);

            DisplayTransferBetweenCustomerAccounts(transferFrom, amountMoney, transferTo);

            TransferMoney(BankAccounts[transferTo].AccountNumber,
                    BankAccounts[transferFrom].AccountNumber, amountMoney);

            UserInterface.DisplayMessage($"{UserName}/CustomerMenu/TransferBetweenCustomerAccounts/AccountOverview/");
            Console.WriteLine("---------------------------------------------------");

            AccountOverview(transferFrom, transferTo);

        }
        public void DisplayTransferBetweenCustomerAccounts(int transferFrom, decimal amountMoney, int transferTo)
        {
            Console.Clear();
            UserInterface.DisplayMessage($"{UserName}/CustomerMenu/TransferBetweenCustomerAccounts/");
            if (transferFrom == 0)
            {
                UserInterface.DisplayMessage($"From: X Amount: X To: X");
            }
            else if (amountMoney == 0)
            {
                UserInterface.DisplayMessage($"From: {transferFrom} Amount: X To: X");
            }
            else if (transferTo == 0)
            {
                UserInterface.DisplayMessage($"From: {transferFrom} Amount: {Utility.AmountDecimal(amountMoney)} To: X");
            }
            else
            {
                UserInterface.DisplayMessage($"From: {transferFrom} Amount: {Utility.AmountDecimal(amountMoney)} To: {transferTo}");
            }
            AccountOverview(); // Shows the Customer their Accounts and the balances in said Accounts

            Console.WriteLine("---------------------------------------------------");
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
            Console.Clear();
        }

        public void InternalMoneyTransfer()
        {
            decimal amountMoney;
            int transferFromWhichAccount;
            decimal transferToWhichAccount;

            Console.Clear();
            AccountOverview();

            Console.WriteLine("Från vilket konto vill du föra över pengarna?");
            transferFromWhichAccount = UserInterface.IsValueNumberCheck(BankAccounts.Count);

            bool correctAmount = false;
            do
            {
                Console.WriteLine("Hur mycket pengar vill du föra över?");
                amountMoney = UserInterface.IsValueNumberCheck();
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
                transferToWhichAccount = UserInterface.IsValueNumberCheck();
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
