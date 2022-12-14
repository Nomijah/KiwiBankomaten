using System;
using System.Collections.Generic;
using System.Linq;

namespace KiwiBankomaten
{
    class DataBase
    {
        // Dictionary for saving users, with 6 test users created.
        public static Dictionary<int, Customer> CustomerDict =
            new Dictionary<int, Customer>()
        {
                {1, new Customer(1, "Tobias", "NotionLover65",false) },
                {2, new Customer(2, "Anas", "Core3.1",false) },
                {3, new Customer(3, "Reidar", "Password123",false) },
                {4, new Customer(4, "Michael", "abc",false) },
                {5, new Customer(5, "Andre", "123",false) },
                {6, new Customer(6, "Ludvig", "drowssaP",false) }
        };

        // List of Admins.
        public static List<Admin> AdminList = new List<Admin>()
        {
            {new Admin("Petter", "Rettep") },
            {new Admin("Max", "Xam") },
            {new Admin("Jonathan", "Nahtajon") },
            {new Admin("Daniel", "Leinad") },
            {new Admin("Charlie", "Eilrahc") }
        };

        // Dictionary with currencies and exchange rates.
        public static Dictionary<string, decimal> ExchangeRates =
            new Dictionary<string, decimal>()
            {
                {"SEK", 1m },
                {"USD", 10.42m },
                {"EUR", 10.85m },
                {"DKK", 1.46m },
                {"NOK", 1.05m },
                {"GBP", 12.59m },
                {"CHF", 11.04m },
                {"AUD", 6.98m },
                {"CNY", 1.45m }
            };

        // Prints out currrency list with current exchange rates.
        public static void PrintCurrencies()
        {
            foreach (string currency in DataBase.ExchangeRates.Keys)
            {
                Console.WriteLine($"{currency}");
            }
        }

        // Dictionary with account types and interest.
        public static Dictionary<string, decimal> BankAccountTypes =
            new Dictionary<string, decimal>
            {
                { "Lönekonto", 0m },
                { "Korttidssparkonto", 1.2m },
                { "Långtidssparkonto", 1.7m },
                { "Barnsparkonto", 2.3m }
            };

        public static Dictionary<string, decimal> LoanAccountTypes =
            new Dictionary<string, decimal>
            {
                { "Bolån", 4.5m },
                { "Billån", 6.3m },
                { "Blancolån", 12.7m }
            };
        public static void ViewAccountTypes(int selection)
        {
            int i = 1;
            switch (selection)
            {
                case 1:
                    foreach (KeyValuePair<string, decimal> type in DataBase.BankAccountTypes)
                    {
                        Console.WriteLine($"-{i} {type.Key} - {type.Value}");
                        i++;
                    }
                    break;
                case 2:
                    foreach (KeyValuePair<string, decimal> type in DataBase.LoanAccountTypes)
                    {
                        Console.WriteLine($"-{i} {type.Key} - {type.Value}");
                        i++;
                    }
                    break;
                default:
                    Console.WriteLine("Ogiltigt värde, det här borde inte kunna hända. Kontakta en admin.");
                    break;
            }
        }
        // Method returns string key of a bank account type based on its index.
        public static string GetKeyFromBankTypeIndex(int index)
        {
            try
            {
                return BankAccountTypes.ElementAt(index).Key;
            }
            // If index does not exist, return an empty string.
            catch
            {
                return "";
            }
        }
        // Method returns string key of a loan account type based on its index.
        public static string GetKeyFromLoanTypeIndex(int index)
        {
            try
            {
                return LoanAccountTypes.ElementAt(index).Key;
            }
            // If index does not exist, return an empty string.
            catch
            {
                return "";
            }
        }

        // Prints out account types with interest values.
        public static void PrintAccountTypes()
        {
            int i = 1;
            foreach (KeyValuePair<string, decimal> type in BankAccountTypes)
            {
                Console.WriteLine($"{i}. {type.Key}, " +
                    $"ränta: {type.Value}");
                i++;
            }
        }

        public static void PrintLoanAccountTypes()
        {
            int i = 1;
            foreach (KeyValuePair<string, decimal> type in LoanAccountTypes)
            {
                Console.WriteLine($"{i}. {type.Key}, " +
                    $"ränta: {type.Value}");
                i++;
            }
        }
    }
}
