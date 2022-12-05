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
            {
                // Print out each account with key, number, name, value and currency
                foreach (KeyValuePair<int, BankAccount> account in BankAccounts)
                {
                    Console.WriteLine($"{account.Key}. {account.Value.AccountNumber} " +
                        $"{account.Value.AccountName}: {Math.Round(account.Value.Amount,2)} " +
                        $"{account.Value.Currency}");
                }
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
            Console.WriteLine("Enter a number as input to navigate in the menu:");
            AccountOverview(); // Shows the Customer their Accounts and the balances in said Accounts

            Console.WriteLine("From which account do you want to transfer money from?: ");
            Utility.IsValueNumberCheck(out transferFromWhichAccount, BankAccounts.Count); // Gets User input and Checks if it's Valid

            Console.WriteLine("How much money do you want to transfer?: ");
            Utility.IsValueNumberCheck(out amountMoney); // Gets User input and Checks if it's Valid

            Console.WriteLine("From which account do you want to transfer money to?: ");
            Utility.IsValueNumberCheck(out transferToWhichAccount, BankAccounts.Count); // Gets User input and Checks if it's Valid

            if (CurrencyCheck(transferToWhichAccount, transferFromWhichAccount))
            {
                // Checks if the giving account has enough funds to go through with the transfer and transfers the money if it's possible 
                TransferFromCheck(transferFromWhichAccount, transferToWhichAccount, amountMoney);
            }
            else
            {
                TransferConvertedCurrency(transferToWhichAccount, transferFromWhichAccount, amountMoney);
            }

        }

        // Checks if transferFromWhichAccount has enough funds to go through with the transfer and then transfers the money if it's possible
        private void TransferFromCheck(int transferFromWhichAccount, int transferToWhichAccount, decimal amountMoney)
        {

            if (BankAccounts[transferFromWhichAccount].Amount >= amountMoney) // Checks if transferFromWhichAccount has enough funds for the transfer
            {
                TransferFromAccToAcc(transferFromWhichAccount, transferToWhichAccount, amountMoney); // Goes through with the transfer
                Console.WriteLine("The Transfer was a success");
                AccountOverview(transferFromWhichAccount, transferToWhichAccount); // Shows the Customer their updated Accounts
            }
            else // If the customer doesn't have enough money
            {
                //Tells the user they dont have enough funds in transferFromWhichAccount 
                Console.WriteLine("Not enough money in Account( {0} );\tMoney in Account( {0} ) - {1}", BankAccounts[transferFromWhichAccount].AccountName, BankAccounts[transferFromWhichAccount].Amount); 
            }

        }
            
        // Transfers the funds between the Accounts
        private void TransferFromAccToAcc(int transferFromWhichAccount, int transferToWhichAccount, decimal amountMoney)
        {

            BankAccounts[transferFromWhichAccount].Amount -= amountMoney; // Removes the funds
            BankAccounts[transferToWhichAccount].Amount += amountMoney; // Adds the funds

        }

        // Method to check if two internal accounts use the same currency.
        private bool CurrencyCheck(int toAccountNum, int fromAccountNum)
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

        // Method for transferring money with currency exchange.
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
            Console.WriteLine("The Transfer was a success");
            AccountOverview(fromAccountNum, toAccountNum); // Shows the Customer their updated Accounts
        }
        public void InternalMoneyTransfer()
        {
            decimal amountMoney;
            int transferFromWhichAccount;
            int transferToWhichAccount;

            Console.Clear();
            AccountOverview();

            Console.WriteLine("Hur mycket pengar vill du föra över?");
            while (!decimal.TryParse(Console.ReadLine(), out amountMoney)) //How much money is being transferred
            {
                Console.WriteLine("Skriv endast siffror");
            }

            if (amountMoney <= 0) //If user enters a negative amount
            {
                Console.WriteLine("Du kan inte föra över en negativ summa");
                return;
            }

            Console.WriteLine("Från vilket konto vill du föra ifrån?");
            while (!int.TryParse(Console.ReadLine(), out transferFromWhichAccount)) //How much money is being transferred
            {
                Console.WriteLine("Skriv endast siffror");
            }

            Console.WriteLine("Skriv det 8-siffriga kontonummer du vill föra över pengar till:");
            while (!int.TryParse(Console.ReadLine(), out transferToWhichAccount)) //How much money is being transferred
            {
                Console.WriteLine("Skriv endast siffror");
            }

            //If chosen amount to transfer is smaller than accountbalance on chosen account
            if (BankAccounts[transferFromWhichAccount].Amount >= amountMoney)
            {
                //If the TransferToOtherUser is true, do the transfer from personal account
                if (TransferToOtherUser(transferToWhichAccount, amountMoney))
                {
                    //Subtract the amount from the users account
                    BankAccounts[transferFromWhichAccount].Amount -= amountMoney;

                    Console.WriteLine($"The amount: {amountMoney} was successfully moved to account: {transferToWhichAccount}");
                }
            }
            //If the chosen amount to transfer exceeds the balance on chosen account
            else
            {
                Console.WriteLine($"Det finns ej tillräckligt med pengar på ditt {BankAccounts[transferFromWhichAccount].AccountName}. Max antal du kan föra över är: {BankAccounts[transferFromWhichAccount].Amount}");
            }

        }

        public bool TransferToOtherUser(int accountNum, decimal transferAmount)
        {
            // Check every user in database
            foreach (Customer customer in DataBase.CustomerDict.Values)
            {
                // Check each account for match
                foreach (BankAccount acc in customer.BankAccounts.Values)
                {
                    // If account is found, add transferAmount to the account and return true
                    if (acc.AccountNumber == accountNum)
                    {
                        acc.Amount = acc.Amount + transferAmount;

                        return true;
                    }
                }
            }
            //If account is not found, returns false
            return false;
        }
    }
}