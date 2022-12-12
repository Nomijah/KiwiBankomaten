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
        // DSaver checks if file exists, if false, creates file and writes databaseinfo.
        public static void DSaver(string fileName)
        {
            // CREATING FILE LOCALLY
            StreamWriter sw = new StreamWriter(fileName);
            // Writes data on file
            if (fileName == "Customers.txt")
            {
                foreach (KeyValuePair<int, Customer> item in DataBase.CustomerDict)
                {
                    sw.Write("Key: " + item.Key +
                    " Id: " + item.Value.Id +
                    " Username: " + item.Value.UserName +
                    " Password: " + item.Value.Password +
                    " Locked: " + item.Value.Locked);
                    sw.WriteLine();
                }
            }
            else if (fileName == "BankAccounts.txt")
            {

            }
            // closes stream
            sw.Close();
            Console.WriteLine("Fil skapades");
        }



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





        // fil 1  customerdictionary  - innehåller 6 st key, id, username, isadmin, locked samt tillhörande värden

        // fil 2
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

