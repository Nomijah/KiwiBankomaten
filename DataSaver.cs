using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Xml.Linq;
using System.Collections;
using System.Net.Sockets;
using SlackAPI;

namespace KiwiBankomaten
{
    public class DataSaver
    {
        // DSaver creates file and writes databaseinfo.
        public static void DSaver(string fileName)
        {
            StreamWriter sw = new StreamWriter(fileName);
            // Writes data on files, according to input name
            if (fileName == "Customers.txt")
            {    
                foreach (KeyValuePair<int, Customer> item in DataBase.CustomerDict)
                {  // Writes information on a single line 
                    sw.Write("Key: " + item.Key +
                    " Id: " + item.Value.Id +
                    " Username: " + item.Value.UserName +
                    " Password: " + item.Value.Password +
                    " Locked: " + item.Value.Locked);
                    sw.WriteLine();
                }
            }
            //        INTERNAL Dictionary<int, BankAccount> BankAccounts;
            else if (fileName == "BankAccounts.txt")
            {
                ////foreach (KeyValuePair<int, BankAccount> item in Customer.BankAccounts) 
                //{
                //sw.Write("Key: " + item.Key +
                //" Id: " + item.Value.Id +
                //" Username: " + item.Value.UserName +
                //" Password: " + item.Value.Password +
                //" Locked: " + item.Value.Locked);
                //sw.WriteLine();
                //}

            }
            //        INTERNAL Dictionary<int, BankAccount> LoanAccounts;
            else if (fileName == "LoanAccounts.txt")
            {
 
            }
            else if (fileName == "Admins.txt")
            { // Writes information on a single line 
                foreach (Admin ad in DataBase.AdminList)
                { // Writes information on a single line 
                    sw.Write("Username: " + ad.UserName
                    + " Password: " + ad.Password);
                    sw.WriteLine();
                }
            }
            else if (fileName == "Currencies.txt")
            {
                foreach (KeyValuePair<string, decimal> item in DataBase.ExchangeRates)
                {  // Writes information on a single line 
                    sw.Write("Currency: " + item.Key +
                    " Value: " + item.Value);
                    sw.WriteLine();
                }
            }
            else if (fileName == "BankAccountTypes.txt")
            {
                foreach (KeyValuePair<string, decimal> item in DataBase.BankAccountTypes)
                { // Writes information on a single line 
                    sw.Write("KontoTyp: " + item.Key +
                    " Ränta: " + item.Value);
                    sw.WriteLine();
                }
            }
            else if (fileName == "LoanAccountTypes.txt")
            {
                foreach (KeyValuePair<string, decimal> item in DataBase.LoanAccountTypes)
                {  // Writes information on a single line 
                    sw.Write("KontoTyp: " + item.Key +
                    " Ränta: " + item.Value);
                    sw.WriteLine();
                }
            }
            sw.Close();   // Closes stream
        }





        // TASKS to complete DataSaver:

        // Åtkomst till internal (Customer.)BankAccounts för att skapa fil i DSaver    
        // Åtkomst till internal (Customer.)LoanAccounts för att skapa fil i DSaver
        // Fil för "log" ? Hur är det tänkt? Räcker det med de andra filerna?
        // Implementera BankAccounts i ShowFile()
        // Implementera LoanAccounts i ShowFile()
        // Implementera ShowFile() som funktion i adminmeny
        // Implementera DataSaver i programmet där det behövs samt när man tryckt exit eller loggat ut
        // ( om någon data ändras, raderas, eller adderas )

        public static void ShowFile()
        {   // Menu for admin, to see the contents of the databasefiles, as chosen
            Console.WriteLine("\nFör att se fil, ange en siffra:");
            Console.WriteLine("1) Adminlista");
            Console.WriteLine("2) CustomerDictionary");
            Console.WriteLine("3) Currencies");
            Console.WriteLine("4) BankaccountTypes");
            Console.WriteLine("5) LoanAccountTypes");
            // After seeing all customers and their specs, we can see their accounts etc, at 6)
            Console.WriteLine("6) Customer specifics, konton med mera");

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
                    DataReading("Currencies.txt");
                    break;
                case "4":
                    DataReading("BankAccountTypes.txt");
                    break;
                case "5":
                    DataReading("LoanAccountTypes.txt");
                    break;
                case "6":
                    DataReading("Customers.txt"); // Writes out all customers specs firstly
                    Console.WriteLine("\nVälj kund för att se konton:");
                    
                    // When we´ve created the files, we can implement them here, so that this makes sense
                    // Ex. Michael= 1).    string cc = 1    --> read all content of 1) in {cc}.text
                    string cc = Console.ReadLine(); 
                    string c = $"{cc}.txt";
                    //DataReading(c);
                    break;
                default:
                    Console.WriteLine("Fel inmatning");
                    break;
            }
        }


        // Method to read and show data from chosen file.
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
            {
                foreach (string line in textLine.Split(' '))
                {
                    // If the list starts with any of these fields, it starts with a new line
                    if (line == "Username:" || line == "Currency:" || line == "KontoTyp:")
                    {
                        Console.WriteLine();
                    }
                    // If the content is data, it will only write the data
                    if (line == "Username:" || line == "Password:" || line == "Locked:" || line == "Id:" ||
                        line == "Key:" || line == "Currency:" || line == "Value:" || line == "KontoTyp:"
                        || line == "Ränta:")
                    {
                        Console.Write(line);
                    }
                    // Otherwise a new line is written
                    else
                    {
                        Console.Write(line + "\n");
                    }
                }
                textLine = sr.ReadLine();
            }
            sr.Close();
        }
    }
}

