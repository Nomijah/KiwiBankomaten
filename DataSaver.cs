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
        // DSaver checks if file exists, if false, creates file and writes databaseinfo
        public static void DSaver()
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
                DataSyncer();
            }
            else
            {
                Console.WriteLine("Hittade fil");
                DataSyncer();
            }
        }

        // Datasyncer could be run throughout the program to sync all data.
        // If the data is not synced it will sync it.
        public static void DataSyncer()
        {
            // Store each line in array of strings of the file, to be able to compare with database
            string[] lines = File.ReadAllLines(@"C:\Users\danie\Desktop\File1.txt");

            // List of string with samevalues, to compare with filelines
            List<string> sameValues = new List<string>();

            bool ValuesAreSynced = false;
            int SyncingPoint = 0;

            // foreach line in file, we will compare to every value of a customers properties
            foreach (string fileLine in lines)
            {
                // if the value is found, we have found same value                    
                foreach (KeyValuePair<int, Customer> item in DataBase.CustomerDict)
                {
                    if (fileLine.Equals("Key: " + item.Key))
                    {
                        sameValues.Add($"Key: {item.Key} is equal");
                        SyncingPoint++;
                    }
                    if (fileLine.Equals("Id: " + item.Value.Id))
                    {
                        sameValues.Add($"Id: {item.Value.Id} is equal");
                        SyncingPoint++;
                    }
                    if (fileLine.Equals("Username: " + item.Value.UserName))
                    {
                        sameValues.Add($"Username: {item.Value.UserName} is equal");
                        SyncingPoint++;
                    }
                    if (fileLine.Equals("Password: " + item.Value.Password))
                    {
                        sameValues.Add($"Password: {item.Value.Password} is equal");
                        SyncingPoint++;
                    }
                    if (fileLine.Equals("Isadmin: " + item.Value.IsAdmin))
                    {
                        sameValues.Add($"Isadmin: {item.Value.IsAdmin} is equal"); 
                        SyncingPoint++;
                        break;   // Using break since it will add every found "isadmin: false", which is 6x
                    }
                    if (fileLine.Equals("Locked: " + item.Value.Locked))
                    {
                        sameValues.Add($"Locked: {item.Value.Locked} is equal");
                        SyncingPoint++;
                        break;
                    }
                }
            }
            // if the value is different the file is not synced with the database
            if (SyncingPoint == DataBase.CustomerDict.Count*6) // 6 items * 6 properties
            {
                ValuesAreSynced= true; 
            }
            if(ValuesAreSynced)
            {
                Console.WriteLine("Synkning är färdig!");
            }
            else
            {  // If the values are not synced in the file, we delete the file and write them to the file
                Console.WriteLine("Alla värden är inte synkade ännu");

                string UnsyncedFile = @"C:\Users\danie\Desktop\File1.txt";
                File.Delete(UnsyncedFile);
                DSaver();
            }
        }



        //Problemet med 1 textfil är att det är text och inte objekt, så det blir svårt att koppla ihop vilken text som tillhör vilken
        //problemet med flera textfiler är att det blir svårt att koppla ihop? eller för många filer

        //Tasks:
        // - primary
        //    X    Make DataSaver possible for other files than File1
        //    X    Connect the "DataBase"-Files to each other.
        //    X    hur välförståeligt behöver detta vara för admin/användare?

        // - secondary
        //    X    Implement DataReading Method to admin menu, to see files and read them     när den funkar fullständigt
        //    X    Make DataSaver not public to the user, only workable       efter all testning, när det funkar
        //    X    Implement DataSaver method in program so that it will sync files during changes and when pressing exit
        //         på passande platser osv.

        //fil 1  customerdictionary  - innehåller 6 st key, id, username, isadmin, locked samt tillhörande värden

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





        // Method to read and show data from chosen file.
        //DataSaver.DataReading("File1");  HOW TO USE
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
    }
}
