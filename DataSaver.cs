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
        public static void DSaver()
        {
            ExistCheck();
        }
        public static void ExistCheck()
        {
            //Starts to check if file exists. If it does not it will create a local file
            string FileOnDesktop = @"C:\Users\danie\Desktop\File1.txt";
            if (!File.Exists(FileOnDesktop))
            {
                Console.WriteLine("Kunde inte hitta fil.\nSkapar en lokal fil.");
                //CREATING FILE LOCALLY
                StreamWriter sw = new StreamWriter(@"C:\Users\danie\Desktop\File1.txt");
                //Writes data on file
                foreach (KeyValuePair<int, Customer> item in DataBase.CustomerDict)
                {
                    sw.WriteLine("Key: " + item.Key );
                    sw.WriteLine("Id: "+ item.Value.Id);
                    sw.WriteLine("Username: "+ item.Value.UserName);
                    sw.WriteLine("Password: "+ item.Value.Password);
                    sw.WriteLine("Isadmin: "+ item.Value.IsAdmin);
                    sw.WriteLine("Locked: "+ item.Value.Locked);
                }
                // closes stream
                sw.Close();
                Console.WriteLine("Fil skapades");
                Console.WriteLine("VI TESTAR .. och nedan läser vi data reading");
            }
            else
            {
                Console.WriteLine("Hittade fil");
                DataSyncer();
            }
        }
        //public static void WriteData(string FileOnDesktop)
        //{
        //    //börja med att skriva ned alla värde i fil som ska skapats


        //}
        public static void DataSyncer()
        { 
            //if the file does not contain the count value of database customerdictionary

            
            //spara fil 
            StreamReader checker = new StreamReader(@"C:\Users\danie\Desktop\File1.txt");
            checker.ReadToEnd();

            //f1.ReadToEnd();//Reads the stream from the current position to the end of the stream.
            //Console.WriteLine("Skriver från skrivbords fil 1 " + f1.ReadToEnd());
            checker.Close();
            if (!checker.Equals(DataBase.CustomerDict.Count()))
            {
                Console.WriteLine("Inte samma värde");                     //ANVÄNDA STREAMREADER OVAN?
                                                                           //Writes values on file  STREAMREADER SKRIVER IN VÄRDE HÄR
                                                                           //If customer in Database.CustomerDict is added- Streamwriter write in file
            }
            else
            {
                Console.WriteLine("Samma värde");
                //LÄMNA?
            }
        }

        ////Method to create file incase there is non already existing
        //public static void CreatingFile()
        //{
        //    using (StreamWriter SWCreator = File.CreateText(@"C:\Users\danie\Desktop\File1.txt"))
        //    {
        //        SWCreator.WriteLine("File created.");
        //    }
        //}
        
        // Method to read and show data from chosen file.           IMPLEMENT TO ADMIN MENU
        public static void DataReading(string name)
        {
            //Takes in parameter which is the name of the file. Example, write "File1" 
            string choice = @"C:\Users\danie\Desktop\" + name + ".txt";
            StreamReader sr = new StreamReader($"{choice}");
            Console.WriteLine($"Content of the File ({name}): \n");
            // This is use to specify from where to start reading input stream
            sr.BaseStream.Seek(0, SeekOrigin.Begin);

            // To read line from input stream
            string str = sr.ReadLine();

            // To read the whole file line by line
            while (str != null)
            {
                Console.WriteLine(str);
                str = sr.ReadLine();
            }
            Console.ReadLine();
            sr.Close();
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
