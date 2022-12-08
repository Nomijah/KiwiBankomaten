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

namespace KiwiBankomaten
{
    public class DataSaver
    {
        //Datasyncer could be run through out the program to sync all data.



        public static void DSaver()
        {
            ExistCheck();
        }
        public static void ExistCheck()
        {
            // Starts to check if file exists. If it does not it will create a local file
            string FileOnDesktop = @"C:\Users\danie\Desktop\File1.txt";
            if (!File.Exists(FileOnDesktop))
            {
                Console.WriteLine("Kunde inte hitta fil.\nSkapar en lokal fil.");
                // CREATING FILE LOCALLY
                StreamWriter sw = new StreamWriter(@"C:\Users\danie\Desktop\File1.txt");
                // Writes data on file
                foreach (KeyValuePair<int, Customer> item in DataBase.CustomerDict)
                {
                    sw.WriteLine("Key: " + item.Key);
                    sw.WriteLine("Id: " + item.Value.Id);
                    sw.WriteLine("Username: " + item.Value.UserName);
                    sw.WriteLine("Password: " + item.Value.Password);
                    sw.WriteLine("Isadmin: " + item.Value.IsAdmin);
                    sw.WriteLine("Locked: " + item.Value.Locked);
                    sw.WriteLine("----------------------");
                }
                // closes stream
                sw.Close();
                Console.WriteLine("Fil skapades");
            }
            else
            {
                Console.WriteLine("Hittade fil");
                DataSyncer();
            }
        }


        //DataSyncer must read to compare if file is same as database value
        //if its the same  --> sync is finished
        //if its not the same ---> write/sync data to file
        public static void DataSyncer()
        {
            //if the file does not contain the count value of database customerdictionary

            StreamReader checker = new StreamReader(@"C:\Users\danie\Desktop\File1.txt");
            checker.ReadToEnd();

            // Store each line in array of strings of the file, to be able to compare with database
            string[] lines = File.ReadAllLines(@"C:\Users\danie\Desktop\File1.txt");

            List<string> sameValues = new List<string>();
            //sameValues.Add("List of data");
            // creates array to be able to compare each customer in database
            //string[,] sameValues = new string[10, 7];

            // foreach line in file, we will compare to every value of a customers properties
            foreach (string fileLine in lines)
            {
                
                foreach (KeyValuePair<int, Customer> item in DataBase.CustomerDict)
                {
                    if (fileLine.Equals("Key: " + item.Key))
                    {
                        sameValues.Add($"Key: {item.Key} is equal");
                    }
                    if (fileLine.Equals("Id: " + item.Value.Id))
                    {
                        sameValues.Add($"Id: {item.Value.Id} is equal");
                    }
                    if (fileLine.Equals("Username: " + item.Value.UserName))
                    {
                        sameValues.Add($"Username: {item.Value.UserName} is equal");
                    }
                    if (fileLine.Equals("Password: " + item.Value.Password))
                    {
                        sameValues.Add($"Password: {item.Value.Password} is equal");
                    }
                    if (fileLine.Equals("Isadmin: " + item.Value.IsAdmin))
                    {
                        sameValues.Add($"Isadmin: {item.Value.IsAdmin} is equal");
                    }
                    if (fileLine.Equals("Locked: " + item.Value.Locked))
                    {
                        sameValues.Add($"Locked: {item.Value.Locked} is equal");
                    }
                }
            }
            foreach (string s in sameValues)
            {
                Console.WriteLine(s);
            }

        }
            ////Asking user to enter the text that we want to write into the MyFile.txt file
            //Console.WriteLine("Enter the Text that you want to write on File");

            //// To read the input from the user
            //string str = Console.ReadLine();

            //// To write the data into the stream
            //sw.Write(str);






        // Method to read and show data from chosen file.           IMPLEMENT TO ADMIN MENU
        public static void DataReading(string name)
        {
            // Takes in parameter which is the name of the file. Example, write "File1" 
            string choice = @"C:\Users\danie\Desktop\" + name + ".txt";
            StreamReader sr = new StreamReader($"{choice}");
            Console.WriteLine($"Content of the File ({name}): \n");

            // This is use to specify from where to start reading input stream
            sr.BaseStream.Seek(0, SeekOrigin.Begin);

            // To read line from input stream
            string textLine = sr.ReadLine();

            // To read the whole file line by line
            while (textLine != null)
            {
                Console.WriteLine(textLine);
                textLine = sr.ReadLine();
            }
            Console.ReadLine();
            sr.Close(); // Must close streamreader after use
        }


        //fil 2
        /// Customer
        /// har dictionary bankokonton
        /// id 4
        /// username michael
        /// password  abc
        /// isadamin false
        /// locked false

        //if property is changed, write on file 2 and save
        //press to read whole customerdict, read on file 2

        // fil 3
        /// Bankaccounts  
        /// bankaccount
        /// int accountnr
        /// string accountname
        /// decimal amount
        /// string currency
        /// decimal interest
        /// list log

        //if account is added  write on file 3 and save
        //if property is changed    write on file 3 and save
        //if log is changed   write on file 3 and save
        // press to read accounts read on file 3

    }
}
