using System;
using System.Collections.Generic;
using System.IO;

namespace KiwiBankomaten
{
    public class DataSaver
    {
        // A list of all database files used for the app.
        internal static List<string> fileNames = new List<string>()
            { "Customers.txt", "BankAccounts.txt", "LoanAccounts.txt",
            "Admins.txt", "ExchangeRates.txt", "BankAccountTypes.txt",
            "LoanAccountTypes.txt"};

        // Checks if DataBase files exists on startup. If they do not,
        // it creates necessary files from DataBase class. If they do
        // exist it writes to DataBase from files.
        public static void LoadDataBase()
        {
            foreach (string fileName in fileNames)
            {
                try
                {
                    // Tries to read from local file. If it doesn't exist, the
                    // catch exception activates and the file is created.
                    StreamReader sr = new StreamReader(fileName);
                    sr.Close();
                    UpdateFromFile(fileName);
                }
                catch (FileNotFoundException)
                {
                    DSaver(fileName);
                }
            }
        }

        // Writes info from DataBase to file.
        public static void DSaver(string fileName)
        {
            StreamWriter sw = new StreamWriter(fileName);
            // Writes data on matching file
            // Customer dictionary.
            if (fileName == "Customers.txt")
            {
                foreach (KeyValuePair<int, Customer> item in DataBase.CustomerDict)
                {
                    sw.Write("Id: {0} Username: {1} " +
                        "Password: {2} Locked: {3}\n",
                        item.Value.Id, item.Value.UserName,
                        item.Value.Password, item.Value.Locked);
                }
            }
            // Each customers bank accounts.
            else if (fileName == "BankAccounts.txt")
            {
                foreach (Customer c in DataBase.CustomerDict.Values)
                {
                    foreach (KeyValuePair<int, BankAccount> item in c.BankAccounts)
                    {
                        sw.Write("Customer ID: {0} Account Key: {1} AccountNr: {2} " +
                            "Name: {3} Amount: {4} Currency: {5} Interest: {6}\n",
                            c.Id, item.Key, item.Value.AccountNumber,
                            item.Value.AccountName, item.Value.Amount,
                            item.Value.Currency, item.Value.Interest);
                        // Add info from log.
                        foreach (Log l in item.Value.LogList)
                        {
                            sw.Write("{0};{1};{2};{3};{4};{5};{6};{7};\n",
                                item.Value.AccountNumber, l.AmountTransferred,
                                l.Currency, l.FromWhichAccount, l.ReceivingMoney,
                                l.TimeOfTransfer, l.ToWhichAccount, c.Id);
                        }
                    }
                }
            }
            // Each customers loan accounts.
            else if (fileName == "LoanAccounts.txt")
            {
                foreach (Customer c in DataBase.CustomerDict.Values)
                {
                    foreach (KeyValuePair<int, LoanAccount> item in c.LoanAccounts)
                    {
                        sw.Write("Customer ID: {0} Account Key: {1} AccountNr: {2} " +
                            "Name: {3} Amount: {4} Currency: {5} Interest: {6}\n",
                            c.Id, item.Key, item.Value.AccountNumber,
                            item.Value.AccountName, item.Value.Amount,
                            item.Value.Currency, item.Value.Interest);
                    }
                }
            }
            // Admin list.
            else if (fileName == "Admins.txt")
            {
                foreach (Admin ad in DataBase.AdminList)
                {
                    sw.Write("Username: " + ad.UserName
                    + " Password: " + ad.Password);
                    sw.WriteLine();
                }
            }
            // Currency dictionary.
            else if (fileName == "ExchangeRates.txt")
            {
                foreach (KeyValuePair<string, decimal> item in DataBase.ExchangeRates)
                {
                    sw.Write("Currency: " + item.Key +
                    " Value: " + item.Value);
                    sw.WriteLine();
                }
            }
            // Bank account type dictionary.
            else if (fileName == "BankAccountTypes.txt")
            {
                foreach (KeyValuePair<string, decimal> item in DataBase.BankAccountTypes)
                {
                    sw.Write("KontoTyp: " + item.Key +
                    " Ränta: " + item.Value);
                    sw.WriteLine();
                }
            }
            // Loan account type dictionary.
            else if (fileName == "LoanAccountTypes.txt")
            {
                foreach (KeyValuePair<string, decimal> item in DataBase.LoanAccountTypes)
                {
                    sw.Write("KontoTyp: " + item.Key +
                    " Ränta: " + item.Value);
                    sw.WriteLine();
                }
            }
            else
            {
                Console.WriteLine("File doesn't exist.");
            }
            // closes stream
            sw.Close();
        }

        // Writes from file to DataBase.
        public static void UpdateFromFile(string fileName)
        {
            StreamReader sr = new StreamReader(fileName);

            // Writes data on matching file.
            if (fileName == "Customers.txt")
            {
                // Clear dictionary.
                DataBase.CustomerDict.Clear();
                // Get each line of document and write to string array.
                string[] lines = sr.ReadToEnd().Split("\n");
                foreach (string item in lines)
                {
                    // Check if line is empty.
                    if (item != "")
                    {
                        // Split line into new string array
                        string[] temp = item.Split(" ");
                        // Write info to DataBase.
                        DataBase.CustomerDict.Add(Convert.ToInt32(temp[1]),
                            new Customer(Convert.ToInt32(temp[1]), temp[3],
                            temp[5], Convert.ToBoolean(temp[7])));
                    }
                }
            }
            else if (fileName == "BankAccounts.txt")
            {
                string[] lines = sr.ReadToEnd().Split("\n");
                // For current account key to use when writing log info.
                int tempKey = 0;
                foreach (Customer c in DataBase.CustomerDict.Values)
                {
                    // Clear dictionary
                    c.BankAccounts.Clear();
                    foreach (string item in lines)
                    {
                        if (item != "")
                        {
                            string[] temp = item.Split(" ");
                            // Check if row contains bank account info.
                            if (temp[0] == "Customer")
                            {
                                // Check that account is owned by Customer and add
                                // to customers bankAccounts if true.
                                if (temp[2] == Convert.ToString(c.Id))
                                {
                                    // Write info to DataBase.
                                    c.BankAccounts.Add(Convert.ToInt32(temp[5]),
                                        new BankAccount(Convert.ToInt32(temp[7]),
                                        temp[9], Convert.ToDecimal(temp[11]),
                                        temp[13], Convert.ToDecimal(temp[15])));
                                    tempKey = Convert.ToInt32(temp[5]);
                                }
                            }
                            // Else it is log connected to previous account.
                            else
                            {
                                string[] logSplit = item.Split(";");
                                // Check that account is owned by Customer and add
                                // to customers bankAccounts if true.
                                if (logSplit[7] == Convert.ToString(c.Id))
                                {
                                    // Makes sure that the log is linked to correct
                                    // account number.
                                    if (Convert.ToInt32(logSplit[0]) ==
                                    c.BankAccounts[tempKey].AccountNumber)
                                    {
                                        c.BankAccounts[tempKey].LogList.Add(
                                            new Log(Convert.ToDecimal(logSplit[1]),
                                            logSplit[2], Convert.ToInt32(logSplit[3]),
                                            Convert.ToBoolean(logSplit[4]),
                                            Convert.ToDateTime(logSplit[5]),
                                            Convert.ToInt32(logSplit[6])));
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else if (fileName == "LoanAccounts.txt")
            {
                string[] lines = sr.ReadToEnd().Split("\n");
                foreach (Customer c in DataBase.CustomerDict.Values)
                {
                    // Clear dictionary
                    c.LoanAccounts.Clear();
                    foreach (string item in lines)
                    {
                        if (item != "")
                        {
                            string[] temp = item.Split(" ");
                            // Check that account is owned by Customer and add
                            // to customers loanAccounts if true.
                            if (temp[2] == Convert.ToString(c.Id))
                            {
                                // Write info to DataBase.
                                c.LoanAccounts.Add(Convert.ToInt32(temp[5]),
                                    new LoanAccount(Convert.ToInt32(temp[7]),
                                    temp[9], Convert.ToDecimal(temp[11]),
                                    temp[13], Convert.ToDecimal(temp[15])));
                            }
                        }
                    }
                }
            }
            else if (fileName == "Admins.txt")
            {
                // Clear list.
                DataBase.AdminList.Clear();
                // Get each line of document and write to string array.
                string[] lines = sr.ReadToEnd().Split("\n");
                foreach (string item in lines)
                {
                    // Check if line is empty.
                    if (item != "")
                    {
                        // Remove rowbreak "\r" at end of string.
                        string fix = item.Remove(item.Length - 1);
                        // Split line into new string array
                        string[] temp = fix.Split(" ");
                        // Write info to DataBase.
                        DataBase.AdminList.Add(new Admin(temp[1], temp[3]));
                    }
                }
            }
            else if (fileName == "ExchangeRates.txt")
            {
                // Clear dictionary.
                DataBase.ExchangeRates.Clear();
                // Get each line of document and write to string array.
                string[] lines = sr.ReadToEnd().Split("\n");
                foreach (string item in lines)
                {
                    // Check if line is empty.
                    if (item != "")
                    {
                        // Split line into new string array
                        string[] temp = item.Split(" ");
                        // Write info to DataBase.
                        DataBase.ExchangeRates.Add(temp[1], Convert.ToDecimal(temp[3]));
                    }
                }
            }
            else if (fileName == "BankAccountTypes.txt")
            {
                // Clear dictionary.
                DataBase.BankAccountTypes.Clear();
                // Get each line of document and write to string array.
                string[] lines = sr.ReadToEnd().Split("\n");
                foreach (string item in lines)
                {
                    // Check if line is empty.
                    if (item != "")
                    {
                        // Split line into new string array
                        string[] temp = item.Split(" ");
                        // Write info to DataBase.
                        DataBase.BankAccountTypes.Add(temp[1], Convert.ToDecimal(temp[3]));
                    }
                }
            }
            else if (fileName == "LoanAccountTypes.txt")
            {
                // Clear dictionary.
                DataBase.LoanAccountTypes.Clear();
                // Get each line of document and write to string array.
                string[] lines = sr.ReadToEnd().Split("\n");
                foreach (string item in lines)
                {
                    // Check if line is empty.
                    if (item != "")
                    {
                        // Split line into new string array
                        string[] temp = item.Split(" ");
                        // Write info to DataBase.
                        DataBase.LoanAccountTypes.Add(temp[1], Convert.ToDecimal(temp[3]));
                    }
                }
            }
            else
            {
                Console.WriteLine("File doesn't exist.");
            }
            sr.Close();
        }


        // This method is not ready yet!!

        // Menu for admin, to see the contents of the databasefiles, as chosen
        public static void ShowFile()
        {
            Console.WriteLine("\nFör att se fil, ange en siffra:");
            Console.WriteLine("1) Adminlista");
            Console.WriteLine("2) CustomerDictionary");
            Console.WriteLine("3) ExchangeRates");
            Console.WriteLine("4) BankaccountTypes");
            Console.WriteLine("5) LoanAccountTypes");
            Console.WriteLine("6) Bankaccounts");

            string input = Console.ReadLine();
            switch (input)
            {
                case "1":
                    DataReading("Admins.txt");
                    break;
                case "2":
                    DataReading("Customers.txt");
                    break;
                case "3":
                    DataReading("ExchangeRates.txt");
                    break;
                case "4":
                    DataReading("BankAccountTypes.txt");
                    break;
                case "5":
                    DataReading("LoanAccountTypes.txt");
                    break;
                case "6":
                    DataReading("BankAccounts.txt");
                    break;
                default:
                    Console.WriteLine("Fel inmatning");
                    break;
            }
        }

        // DataSaver.DataReading("Customer.txt");  HOW TO USE
        public static void DataReading(string fileName)
        {
            StreamReader sr = new StreamReader($"{fileName}");
            Console.WriteLine($"Innehåll av filen '{fileName.ToString()}':");

            // This is use to specify from where to start reading input stream
            sr.BaseStream.Seek(0, SeekOrigin.Begin);

            // To read line from input stream
            string textLine = sr.ReadLine();

            // To read the whole file line by line
            while (textLine != null)
                Console.WriteLine("------------------------");
            foreach (string line in textLine.Split(' '))
            {
                // If the list starts with any of these fields, it starts with a new line
                //if (line == "Username:" || line == "Currency:" || line == "KontoTyp:" || line == "Customer ID:" )
                //{
                //    Console.WriteLine();
                //}
                // If the content is data, it will only write the data
                if (line == "Username:" || line == "Password:" || line == "Locked:" || line == "Id:" ||
                    line == "Key:" || line == "Currency:" || line == "Value:" || line == "KontoTyp:"
                    || line == "Ränta:" || line == "Customer ID:" || line == "AccountNr:"
                    || line == "Name:" || line == "Lönekonto:" || line == "Amount:" || line == "Interest:"
                    || line == "Account Key:")
                {
                    Console.Write(line);
                }
                // Otherwise a new line is written
                else
                {
                    Console.Write(line + "\n");
                }
            }
            Console.WriteLine(textLine);
            textLine = sr.ReadLine();
            sr.Close();
        }
    }
}

