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
                                $"{DataBase.BankAccountTypes[userChoice].Item1}. " +
                                $"med ränta " +
                                $"{DataBase.BankAccountTypes[userChoice].Item2}%." +
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
            foreach (var item in BankAccounts.Values)
            {
                Console.WriteLine($"-{index}) -\tKontoNamn : {item.AccountName} - KontoSaldo : {item.Amount} {item.Currency}");
                index++;
            }

        }
        public void TransferBetweenCustomerAccounts()
        {
            decimal amountMoney;
            int transferFromWhichAccount;
            int transferToWhichAccount;

            Console.Clear();
            AccountOverview();

            Console.WriteLine("How much money do you want to transfer?: ");
            Program.IsValueNumber(out amountMoney);

            Console.WriteLine("From which account do you want to transfer money from?: ");
            Program.IsValueNumber(out transferFromWhichAccount, 1, BankAccounts.Count);

            Console.WriteLine("From which account do you want to transfer money to?: ");
            Program.IsValueNumber(out transferToWhichAccount, 1, BankAccounts.Count);

            if (InternalCurrencyCheck(transferToWhichAccount, transferFromWhichAccount))
            {
                TransferFromCheck(transferFromWhichAccount, transferToWhichAccount, amountMoney);
            }
            else
            {
                TransferConvertedCurrency(transferToWhichAccount, transferFromWhichAccount, amountMoney);
            }

        }
        private void TransferFromCheck(int transferFromWhichAccount, int transferToWhichAccount, decimal amountMoney)
        {
            if (BankAccounts[transferFromWhichAccount].Amount >= amountMoney)
            {
                TransferFromAccToAcc(transferFromWhichAccount, transferToWhichAccount, amountMoney);
                Console.WriteLine("The Transfer was a success");
                AccountOverview();
            }
            else
            {
                Console.WriteLine("Not enough money in Account( {0} );\tMoney in Account( {0} ) - {1}", BankAccounts[transferFromWhichAccount].AccountName, BankAccounts[transferFromWhichAccount].Amount);
            }
        }
        private void TransferFromAccToAcc(int transferFromWhichAccount, int transferToWhichAccount, decimal amountMoney)
        {
            BankAccounts[transferFromWhichAccount].Amount -= amountMoney;
            BankAccounts[transferToWhichAccount].Amount += amountMoney;


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