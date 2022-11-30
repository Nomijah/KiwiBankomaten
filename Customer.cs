using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;


namespace KiwiBankomaten
{
    internal class Customer : User
    {
        private Dictionary<int, BankAccount> BankAccounts;

        // Used for creating test customers
        public Customer(int id, string username, string password)
        {
            Id = id;
            UserName = username;
            Password = password;
            IsAdmin = false;

            // Bankaccounts for testing, same for each user
            BankAccounts = new Dictionary<int, BankAccount>()
            {
                { 1, new BankAccount("Lönekonto", 25347.34m, "SEK", 0m) },
                { 2, new BankAccount("Sparkonto", 324000m, "SEK", 2.5m) },
                { 3, new BankAccount("Utlandskonto", 74654.36m, "EUR", 1.3m) },
                { 4, new BankAccount("Företagskonto", 624.86m, "USD", 0m) }
            };
        }

        // Use this when creating customers in program
        public Customer(string username, string password)
        {
            if (DataBase.UserDict == null)
            {
                Id = 1;
            }
            else
            {
                int newId = DataBase.UserDict.Last().Key + 1;
                Id = newId;
            }
            UserName = username;
            Password = password;
            IsAdmin = false;

            BankAccounts = new Dictionary<int, BankAccount>()
            {
                { 1, new BankAccount("Lönekonto", "SEK", 1m) }
            };
        }

        public override void ViewAccounts()
        {
            foreach (KeyValuePair<int, BankAccount> account in BankAccounts)
            {
                Console.WriteLine($"{account.Value.AccountNumber} {account.Value.AccountName}: " +
                    $"{account.Value.Amount} {account.Value.Currency}");
            }
        }

        public void OpenAccount()
        {
            decimal interest = ChooseAccountType();
            string accountName = ChooseAccountName();
            string currency = ChooseCurrency();
            // gets the highest key present and adds one to get new key
            int index = BankAccounts.Keys.Max() + 1;
            BankAccounts.Add(index, new BankAccount(accountName, currency, interest));
            InsertMoneyIntoNewAccount(interest);
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
            Console.WriteLine("Mängden du kommer tjäna på ränta i ditt nya konto : ");
            Console.WriteLine("1 år : " + interestAmount);
            for (int i = 0; i < 5; i++)
            {
                insertAmount += interestAmount;
                interestAmount = insertAmount * interest / 100;
            }
            Console.WriteLine("5 år : " + interestAmount);
            for (int i = 0; i < 5; i++)
            {
                insertAmount += interestAmount;
                interestAmount = insertAmount * interest / 100;
            }
            Console.WriteLine("10 år : " + interestAmount);
        }
        private static string ChooseCurrency()
        {
            Console.Clear();
            Console.WriteLine("Vilken valuta vill du använda till ditt konto?" +
                "\nTillgängliga valutor:");

            DataBase.PrintCurrencies();

            Console.Write("Ange valuta: ");
            string currency = Console.ReadLine().ToUpper();
            // Check if user input is correct
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

        private static string ChooseAccountName()
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
                } while (answer != "J" && answer != "N");
            } while (notReady);
            return accountName;
        }

        public static decimal ChooseAccountType()
        {
            Console.Clear();
            int userChoice = 0;
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
                                        "välj [J] för ja eller N för nej.");
                                    break;
                            }
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
            return DataBase.BankAccountTypes[userChoice - 1].Item2;
        }

        public void AccountOverview()
        {
            int index = 1;
            foreach (var item in BankAccounts.Values) // Loops through all the Customers Accounts
            {              
                // Displays to the Customer their Account
                Console.WriteLine($"-{index}) -\tKontoNamn : {item.AccountName} - KontoSaldo : {Math.Round(item.Amount,2)} {item.Currency}");
                index++;
            }

        }
        public void TransferBetweenCustomerAccounts()
        {
            decimal amountMoney;
            int transferFromWhichAccount;
            int transferToWhichAccount;
            bool isNumber = false;

            Console.Clear();
            Console.WriteLine("Enter a number as input to navigate in the menu:");
            AccountOverview(); // Shows the Customer their Accounts and the balances in said Accounts

            Console.WriteLine("How much money do you want to transfer?: ");
            Program.IsValueNumberCheck(out amountMoney, isNumber); // Gets User input and Checks if it's Valid

            Console.WriteLine("From which account do you want to transfer money from?: ");
            Program.IsValueNumberCheck(out transferFromWhichAccount, 1, BankAccounts.Count, isNumber); // Gets User input and Checks if it's Valid

            Console.WriteLine("From which account do you want to transfer money to?: ");
            Program.IsValueNumberCheck(out transferToWhichAccount, 1, BankAccounts.Count, isNumber); // Gets User input and Checks if it's Valid

            if (InternalCurrencyCheck(transferToWhichAccount, transferFromWhichAccount))
            {
                // Checks if the giving account has enough funds to go through with the transfer and transfers the money if it's possible 
                TransferFromCheck(transferFromWhichAccount, transferToWhichAccount, amountMoney);
            }
            else
            {
                TransferConvertedCurrency(transferToWhichAccount, transferFromWhichAccount, amountMoney);
            }

        }
        private void TransferFromCheck(int transferFromWhichAccount, int transferToWhichAccount, decimal amountMoney) // Checks if transferFromWhichAccount has enough funds to go through with the transfer and then transfers the money if it's possible
        {
            // transferFromWhichAccount == Contains which Account to transfer from // transferToWhichAccount == Contains which Account to transfer to // amountMoney = Contains the quantity that is to be transfered
            if (BankAccounts[transferFromWhichAccount].Amount >= amountMoney) // Checks if transferFromWhichAccount has enough funds for the transfer
            {
                TransferFromAccToAcc(transferFromWhichAccount, transferToWhichAccount, amountMoney); // Goes through with the transfer
                Console.WriteLine("The Transfer was a success");
                AccountOverview(); // Shows the Customer their updated Accounts
            }
            else // If the customer doesn't have enough money
            {
                Console.WriteLine("Not enough money in Account( {0} );\tMoney in Account( {0} ) - {1}", BankAccounts[transferFromWhichAccount].AccountName, BankAccounts[transferFromWhichAccount].Amount); //Tells the user they dont have enough funds in transferFromWhichAccount 
            }
        }
        private void TransferFromAccToAcc(int transferFromWhichAccount, int transferToWhichAccount, decimal amountMoney) // Transfers the funds between the Accounts
        {
            // transferFromWhichAccount == Contains which Account to transfer from // transferToWhichAccount == Contains which Account to transfer to // amountMoney = Contains the quantity that is to be transfered
            BankAccounts[transferFromWhichAccount].Amount -= amountMoney; // Removes the funds
            BankAccounts[transferToWhichAccount].Amount += amountMoney; // Adds the funds


        }

        // Method to check if two internal accounts use the same currency
        private bool InternalCurrencyCheck(int toAccountNum, int fromAccountNum)
        {
            if (BankAccounts[toAccountNum].Currency ==
                BankAccounts[fromAccountNum].Currency)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // Method for transferring money with currency exchange
        public void TransferConvertedCurrency(int toAccountNum,
            int fromAccountNum, decimal amountMoney)
        {
            decimal toRate = 1;
            decimal fromRate = 1;
            // Check which currency the accounts are in
            foreach (KeyValuePair<string, decimal> item in DataBase.ExchangeRates)
            {
                // When match is found, save conversion rate
                if (item.Key == BankAccounts[toAccountNum].Currency)
                {
                    toRate = item.Value;
                }
                if (item.Key == BankAccounts[fromAccountNum].Currency)
                {
                    fromRate = item.Value;
                }
            }

            // Withdraw from the source account
            BankAccounts[fromAccountNum].Amount -= amountMoney;
            // Add converted value to target account
            BankAccounts[toAccountNum].Amount += (amountMoney / toRate) * fromRate;
        }
    }
}