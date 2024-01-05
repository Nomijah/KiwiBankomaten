﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace KiwiBankomaten
{
    internal class Customer : User
    {
        internal Dictionary<int, BankAccount> BankAccounts;
        internal Dictionary<int, LoanAccount> LoanAccounts;

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
            LoanAccounts = new Dictionary<int, LoanAccount>()
            {
                {1, new LoanAccount("Bolån", -1000000m, 4.5m) }
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
            Locked = false;

            BankAccounts = new Dictionary<int, BankAccount>()
            {
                { 1, new BankAccount("Lönekonto", "SEK", 0m) }
            };
            LoanAccounts = new Dictionary<int, LoanAccount>();
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
            int typeIndex = -1;
            bool answer = false;
            UserInterface.CurrentMethodMagenta($"{UserName}/CustomerMenu/" +
                $"CreateAccount/");
            UserInterface.CurrentMethod("Vilken typ av konto vill du öppna?");
            DataBase.PrintAccountTypes();
            while (!DataBase.BankAccountTypes.ContainsKey(DataBase.GetKeyFromBankTypeIndex(typeIndex)) || !answer)
            {

                Int32.TryParse(UserInterface.PromptForString(), out typeIndex);
                typeIndex -= 1;
                if (!DataBase.BankAccountTypes.ContainsKey(DataBase.GetKeyFromBankTypeIndex(typeIndex)))
                {
                    UserInterface.CurrentMethodRed("Felaktigt val, kontotypen du angett " +
                        "finns inte i listan.");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(6);
                }
                else
                {
                    // Check if user is happy with the choice
                    answer = !Utility.YesOrNo($"Ditt konto får Kontotyp [{DataBase.GetKeyFromBankTypeIndex(typeIndex)}]",
                $"Vill du godkänna detta? [J/N]");
                    if (answer == false)
                    {
                        Utility.RemoveLines(8); 
                    }
                }
            }

            // returns the interest rate of chosen account type
            return DataBase.BankAccountTypes[DataBase.GetKeyFromBankTypeIndex(typeIndex)];
        }

        // Lets the customer choose a name for the account.
        private string ChooseAccountName()
        {
            string accountName = "";
            bool answer = false;
            Utility.RemoveLinesVariable(13, DataBase.BankAccountTypes.Count - 1);
            
            UserInterface.CurrentMethod("Vilket namn vill du ge ditt konto?");
            while (accountName == "" || !answer)
            {
                UserInterface.PromptForString(out accountName);
                if (accountName == "")
                {
                    UserInterface.CurrentMethodRed($"Ett namn måste ha minst En karaktär. Ditt namn [{accountName}]");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(6);
                }
                else
                {
                    answer = !Utility.YesOrNo($"Ditt konto får Namn [{accountName}]", $"Vill du godkänna detta? [J/N]");
                    if (answer == false)
                    {
                        Utility.RemoveLines(8);
                    }
                }
            }
            return accountName;
        }

        // Lets the customer choose which currency the account shall be in.
        private string ChooseCurrency()
        {
            string currency;
            Utility.RemoveLines(10);
            
            UserInterface.CurrentMethod("Vilken valuta vill du använda till ditt konto?");
            DataBase.PrintCurrencies();
            do
            {

                // Check if user input is correct, if not ask again
                while (!DataBase.ExchangeRates.ContainsKey(UserInterface.PromptForString(out currency).ToUpper()))
                {
                    UserInterface.CurrentMethodRed("Valutan du angett finns inte i systemet. " +
                        "Välj en Valuta från listan");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(6);
                } 
            } while (Utility.YesOrNo($"Ditt konto får Valuta [{currency.ToUpper()}]",
                $"Vill du godkänna detta? [J/N]"));

            return currency.ToUpper();
        }

        // Method for adding money into newly created account.
        public void InsertMoneyIntoNewAccount(decimal interest)
        {
            // Used to ensure money amount is valid, has to be a positive number.
            bool noError;
            // Amount of money to be inserted into new account.
            decimal insertAmount;

            Utility.RemoveLinesVariable(13, DataBase.ExchangeRates.Count - 1);

            // Will only proceed if user selects J or N.
            if (Utility.YesOrNo($"Vill du sätta in [{BankAccounts[BankAccounts.Keys.Max()].Currency}] i ditt nya konto? [J/N]"))
            {
                return;
            }

            // Loop runs until user has chosen a valid amount of money to put into account.
            do
            {
                noError = true;
                // Checks if amount to be inserted is a number and checks if it is a positive number.
                if (decimal.TryParse(UserInterface.QuestionForString("Skriv in mängden pengar du vill sätta in"), out insertAmount) && insertAmount >= 0)
                {
                    // Adds money into newly created account.
                    BankAccounts[BankAccounts.Keys.Max()].Amount += insertAmount;
                    // Adds this transfer to the logbook with date, money amount and which account the money was sent to.
                    BankAccounts[BankAccounts.Keys.Max()].LogList.Add(new Log
                        (insertAmount, BankAccounts[BankAccounts.Keys.Max()].AccountNumber));
                }
                else
                {
                    UserInterface.CurrentMethodRed("Det där är inte ett giltigt värde");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(8);
                    noError = false;
                }
            } while (noError == false);
            ViewInterestSavingsOfNewAccount(interest, insertAmount);
        }

        // Method for printing the amount of money the account's interest will earn
        // them over different amounts of time with the money they've just inserted.
        public void ViewInterestSavingsOfNewAccount(decimal interest, decimal insertAmount)
        {
            decimal interestAmount = insertAmount * interest / 100;
            UserInterface.CurrentMethod($"Såhär mycket ränta kommer du tjäna med {Utility.AmountDecimal(insertAmount)} {BankAccounts.Last().Value.Currency}");
            Console.WriteLine(" +-----------------------------------------------------------------------------------+");
            Console.Write(" |");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("1 år : " + Math.Round(interestAmount, 2));
            Console.ForegroundColor = ConsoleColor.White;
            Utility.MoveCursorTo(85);

            // Calculates amount of money account will earn in interest in 5 years.
            for (int i = 0; i < 4; i++)
            {
                interestAmount += (insertAmount + interestAmount) * interest / 100;
            }
            Console.Write(" |");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("5 år : " + Math.Round(interestAmount, 2));
            Console.ForegroundColor = ConsoleColor.White;
            Utility.MoveCursorTo(85);

            // Calculates amount of money account will earn in interest in 10 years.
            for (int i = 0; i < 5; i++)
            {
                interestAmount += (insertAmount + interestAmount) * interest / 100;
            }
            Console.Write(" |");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write("10 år : " + Math.Round(interestAmount, 2));
            Console.ForegroundColor = ConsoleColor.White;
            Utility.MoveCursorTo(85);


        }

        // Prints out users bank accounts.
        public void BankAccountOverview()
        {
            
            // Print out each account with key, number, name, value and currency
            UserInterface.CurrentMethod("Bankkonton:");

            foreach (KeyValuePair<int, BankAccount> account in BankAccounts)
            {
                Console.Write(" |");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"-{account.Key}). {account.Value.AccountNumber} " +
                    $"{account.Value.AccountName}: {Utility.AmountDecimal(account.Value.Amount)} " +
                    $"{account.Value.Currency}");
                Console.ForegroundColor = ConsoleColor.White;
                Utility.MoveCursorTo(85);
            }
        }

        // Prints out users loan accounts.
        public void LoanAccountOverview()
        {
            UserInterface.CurrentMethod("Lånekonton:");            

            // Print out each loan account with key, number, value and currency
            foreach (KeyValuePair<int, LoanAccount> account in LoanAccounts)
            {
                Console.Write(" |");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"-{account.Key}). {account.Value.AccountNumber} " +
                    $"{account.Value.AccountName}: {Utility.AmountDecimal(account.Value.Amount)} " +
                    $"{account.Value.Currency}");
                Console.ForegroundColor = ConsoleColor.White;
                Utility.MoveCursorTo(85);

            }
        }
 
        // Shows the Customer the Accounts that was involved in the transaction
        public void AccountOverview(int fromWhichAccount, int toWhichAccount)
        {
            UserInterface.CurrentMethod("Pengarna skickades från: ");
            Console.Write(" |");
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Write($"KontoNamn : {BankAccounts[fromWhichAccount].AccountName} - KontoSaldo : " +
                $"{Utility.AmountDecimal(BankAccounts[fromWhichAccount].Amount)} {BankAccounts[fromWhichAccount].Currency}");
            Console.ForegroundColor = ConsoleColor.White;
            Utility.MoveCursorTo(85);

            UserInterface.CurrentMethod("Pengar skickades till: ");
            Console.Write(" |");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"KontoNamn : {BankAccounts[toWhichAccount].AccountName} - KontoSaldo : " +
                $"{Utility.AmountDecimal(BankAccounts[toWhichAccount].Amount)} {BankAccounts[toWhichAccount].Currency}");
            Console.ForegroundColor = ConsoleColor.White;
            Utility.MoveCursorTo(85);
        }

        //Initalizes the transfer between accounts
        public void TransferBetweenCustomerAccounts()
        {
            
            decimal amountMoney = 0;
            int transferFrom = 0;
            int transferTo = 0;

            UserInterface.CurrentMethodMagenta($"{UserName}/CustomerMenu/" +
                    $"TransferBetweenCustomerAccounts/");

            DisplayTransferBetweenCustomerAccounts(transferFrom, amountMoney, transferTo);

            UserInterface.QuestionForIntMax("Från vilket konto vill du föra över pengarna?",out transferFrom, BankAccounts.Count);

            DisplayTransfer(transferFrom, amountMoney, transferTo);

            CheckAccountValue(transferFrom, out amountMoney);

            DisplayTransfer(transferFrom, amountMoney, transferTo);

            // Gets User input and Checks if it's Valid
            UserInterface.QuestionForIntMax("Vilket konto vill du föra över pengarna till?", out transferTo, BankAccounts.Count);

            DisplayTransfer(transferFrom, amountMoney, transferTo);

            TransferMoney(BankAccounts[transferTo].AccountNumber,
                    BankAccounts[transferFrom].AccountNumber, amountMoney);

            UserInterface.CurrentMethodMagenta($"{UserName}/CustomerMenu/" +
                $"TransferBetweenCustomerAccounts/AccountOverview/");

            AccountOverview(transferFrom, transferTo);

        }
        public void DisplayTransfer(int transferFrom,
            decimal amountMoney, int transferTo)
        {
            Utility.RemoveLinesVariable(9, BankAccounts.Count - 1);
            DisplayTransferBetweenCustomerAccounts(transferFrom, amountMoney, transferTo);
        }

        public void DisplayTransferBetweenCustomerAccounts(int transferFrom, 
            decimal amountMoney, int transferTo)
        {
            Console.WriteLine(" +-----------------------------------------------------------------------------------+");

            if (transferFrom == 0)
            {
                Console.Write(" |");
                Console.Write($"Från: X ");
                Console.Write($"Pengar: X ");
                Console.Write($"Till: X");
            }
            else if (amountMoney == 0)
            {
                Console.Write(" |");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Från: {transferFrom} ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"Pengar: X ");
                Console.Write($"Till: X");

            }
            else if (transferTo == 0)
            {
                Console.Write(" |");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Från: {transferFrom} ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"Pengar: {Utility.AmountDecimal(amountMoney)} ");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write($"Till: X");

            }
            else
            {
                Console.Write(" |");
                Console.ForegroundColor = ConsoleColor.Red;
                Console.Write($"Från: {transferFrom} ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.Write($"Pengar: {Utility.AmountDecimal(amountMoney)} ");
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.Write($"Till: {transferTo}");
            }
            
            Console.ForegroundColor = ConsoleColor.White;
            Utility.MoveCursorTo(85);
            BankAccountOverview(); // Shows the Customer their Accounts and the balances in said Accounts

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
                        // Adds transaction to log.
                        b.LogList.Add(new Log(amountMoney, fromAccountNum));
                    }
                    else if (fromAccountNum == b.AccountNumber)
                    {
                        // Withdraw from the source account.
                        b.Amount -= amountMoney;
                        // Adds transaction to log.
                        b.LogList.Add(new Log(amountMoney,fromAccountNum, toAccountNum));
                    }
                }
            }
            UserInterface.CurrentMethodGreen("Överföringen lyckades.");
            Utility.PressEnterToContinue();
            Utility.RemoveLinesVariable(11, BankAccounts.Count - 1);
        }

        public void InternalMoneyTransfer()
        {
            decimal amountMoney = 0;
            int transferFrom = 0;
            int transferTo = 0;

            UserInterface.CurrentMethodMagenta($"{UserName}/CustomerMenu/" +
                    $"InternalMoneyTransfer/");

            //visar meddelande
            DisplayTransferBetweenCustomerAccounts(transferFrom, amountMoney, transferTo);

            UserInterface.QuestionForIntMax("Från vilket konto vill du föra över pengarna?", out transferFrom, BankAccounts.Count);

            DisplayTransfer(transferFrom, amountMoney, transferTo);

            CheckAccountValue(transferFrom, out amountMoney);

            DisplayTransfer(transferFrom, amountMoney, transferTo);

            bool correctAccountNumber = false;
            do
            {
                UserInterface.QuestionForIntMax("Skriv det 8-siffriga kontonummer du vill föra över pengar till:", out transferTo, 100000000);

                DisplayTransfer(transferFrom, amountMoney, transferTo);

                //om correctAcountNumber inte är sant
                correctAccountNumber = CheckIfAccountExists(transferTo);
                if (!correctAccountNumber)
                {
                    if (!Utility.ContinueOrAbort())
                    {
                        return;
                    }
                    else
                    {
                        Utility.RemoveLines(4);
                    }
                }
            } while (!correctAccountNumber);

            //överföringsmmetod med inparameter (bankkontot)
            TransferMoney(transferTo,
            BankAccounts[transferFrom].AccountNumber, amountMoney);
            
            UserInterface.CurrentMethodMagenta($"{UserName}/CustomerMenu/" +
                $"InternalMoneyTransfer/BankAccountOverview/");

            BankAccountOverview();

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
            UserInterface.CurrentMethodRed("Kontot du har angett finns inte i banken.");
            return false;
        }

        // Checks if account holds the chosen amount
        public void CheckAccountValue(int accountNum, out decimal amountMoney)
        {
            bool correctAmount = false;
            do
            {
                UserInterface.QuestionForDecimal("Hur mycket pengar vill du föra över?", out amountMoney);
                
                if (amountMoney > BankAccounts[accountNum].Amount)
                {
                    UserInterface.CurrentMethodRed("Summan du har angett finns inte på kontot, " +
                        "försök igen.");
                    Utility.ContinueOrAbort();
                    Utility.RemoveLines(8);
                }
                else
                {
                    correctAmount = true;
                }

            } while (!correctAmount);
        }

        // For customer to loan money
        public void LoanMoney()
        {
            // Gets loan type from user            
            string userChoice = ChooseLoanAccountType();
            decimal amountMoney;

            UserInterface.CurrentMethod("Hur mycket pengar vill du låna?" +
                    $" Du kan max låna {Utility.AmountDecimal(CheckLoanLimit())} kronor.");
            do
            {
                while ((amountMoney = UserInterface.IsValueNumberCheck()) > CheckLoanLimit())
                {
                    UserInterface.CurrentMethodRed($"Du kan inte låna mer än" +
                        $"{Utility.AmountDecimal(CheckLoanLimit())} kronor");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(6);
                }
            } while (Utility.YesOrNo($"Du tar nu ett Lån på {Utility.AmountDecimal(amountMoney)} kronor", "Vill du godkänna detta? [J/N]")) ;

            // Gets the highest key present and adds one to get new key
            int index;
            if (LoanAccounts.Count < 1)
            {
                index = 1;
            }
            else
            {
                index = LoanAccounts.Keys.Max() + 1;
            }
            // Adds the new loan account to customers loan account dictionary
            LoanAccounts.Add(index, new LoanAccount(userChoice,
                amountMoney - (amountMoney * 2), DataBase.LoanAccountTypes[userChoice]));
            // Adds the loaned amount to customers standard account
            BankAccounts[1].Amount += amountMoney;

            UserInterface.CurrentMethodGreen($"Summan har nu anlänt på {BankAccounts[1].AccountName}");
            UserInterface.CurrentMethod("Nytt lånekonto har skapats.");

            // Adds transaction to log, first bank account is shown as
            // having received money from the loan account.
            BankAccounts[1].LogList.Add(new Log(amountMoney, LoanAccounts[index].AccountNumber));

            LoanAccountOverview();

        }

        // Method for choosing what type of loan account.
        public string ChooseLoanAccountType()
        {
            bool answer = false;
            int typeIndex = -1;

            UserInterface.CurrentMethodMagenta($"{UserName}/CustomerMenu/" +
                    $"LoanMoney/");
            UserInterface.CurrentMethod("Vilken typ av konto vill du öppna?");
            DataBase.PrintLoanAccountTypes();
            // Loop until user has entered a valid choice
            while (!DataBase.LoanAccountTypes.ContainsKey(DataBase.GetKeyFromLoanTypeIndex(typeIndex)) || !answer)
            {
                
                Int32.TryParse(UserInterface.PromptForString(), out typeIndex);
                typeIndex -= 1;
                if (!DataBase.LoanAccountTypes.ContainsKey(DataBase.GetKeyFromLoanTypeIndex(typeIndex)))
                {
                    UserInterface.CurrentMethodRed("Felaktigt val, kontotypen du angett " +
                        "finns inte i listan.");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(6);
                }
                else
                {
                    // Check if user is happy with the choice
                    answer = !Utility.YesOrNo($"Ditt konto får Kontotyp [{DataBase.GetKeyFromLoanTypeIndex(typeIndex)}]",
                $"Vill du godkänna detta? [J/N]");
                }
            }

            // returns the interest rate of chosen account type

            return DataBase.GetKeyFromLoanTypeIndex(typeIndex);
        }

        // Returns the maximum loan amount of the customer
        public decimal CheckLoanLimit()
        {
            decimal sum = 0;

            // Sums all the users values together
            foreach (BankAccount item in BankAccounts.Values)
            {
                if (item.Currency != "SEK")
                {
                    sum += ConvertToSek(item);
                }
                else
                {
                    sum += item.Amount;
                }
            }

            // Subtracts current loans as the values in LoanAccounts is negative
            foreach (LoanAccount item in LoanAccounts.Values)
            {
                sum += item.Amount/5;
            }

            return sum * 5;
        }
        // Converts foreign currency to SEK
        public decimal ConvertToSek(BankAccount userAccount)
        {
            foreach (KeyValuePair<string, decimal> item in DataBase.ExchangeRates)
            {
                if (item.Key == userAccount.Currency)
                {
                    return userAccount.Amount * item.Value;
                }
            }
            return 0;
        }
        // User selects which bank account they want to view the log for.
        public void ViewLog()
        {
            bool noError;
            int accountChoice;
            List<Log> reversedList;
            UserInterface.CurrentMethodMagenta($"{UserName}/CustomerMenu/ViewLogs");
            BankAccountOverview();
            do
            {
                UserInterface.CurrentMethod("Vilket konto vill du se överföringslogg på?");
                noError = Int32.TryParse(UserInterface.PromptForString(), out accountChoice);
                if (!noError || !BankAccounts.Keys.Contains(accountChoice))
                {
                    UserInterface.CurrentMethodRed("Kontot du valde existerar inte. Skriv in en giltig siffra.");
                    Utility.PressEnterToContinue();
                    Utility.RemoveLines(8);
                }
            } while (!noError || !BankAccounts.Keys.Contains(accountChoice));
            // We copy the LogList to a new one to ensure we never alter the original.
            if (BankAccounts[accountChoice].LogList == null)
            {
                Console.WriteLine("Listan har inte instansierats. " +
                    "Du borde inte kunna se det här. Kontakta en admin O_o.");
            }
            else if (BankAccounts[accountChoice].LogList.Count < 1)
            {
                UserInterface.CurrentMethodRed("Här var det tomt -_-");
            }
            else if(BankAccounts[accountChoice].LogList != null)
            {
                reversedList = new List<Log>(BankAccounts[accountChoice].LogList);
                reversedList.Reverse();
                foreach (Log l in reversedList)
                {
                    l.PrintLog();
                }
                // Reversed list is un-reversed so we don't print out the wrong order next time.
                reversedList.Reverse();
            }
        }
    }
}
