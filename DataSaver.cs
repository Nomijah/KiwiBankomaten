using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using static System.Net.Mime.MediaTypeNames;
using System.ComponentModel;


namespace KiwiBankomaten
{
    public class DataSaver
    {
        public void saver()
        {
            StreamReader f1 = new StreamReader(@"C:\Users\danie\Desktop\File1.txt");
            Console.WriteLine("Skriver från skrivbords fil 1 " + f1.ReadToEnd());
            f1.Close();
            //f1.ReadToEnd();//Reads the stream from the current position to the end of the stream.
                           // f1.WriteLine();// This method is used to write data to a text stream with a newline.
                           //Börja med att skapa fil lokalt som FileOne/osv
                           //börja med att skriva ned alla värde i fil som ska skapats
                           //spara fil
            StreamReader RepoFileOne = new StreamReader(@"C:\Users\danie\source\repos\KiwiBankomaten\bin\Debug\netcoreapp3.1\FileOne.txt");
            Console.WriteLine("\nSkriver från repofileone från repo");
            Console.WriteLine(RepoFileOne.ReadToEnd());
            RepoFileOne.Close();
        }

        //Allt måste läsas och sparas


        //fil 1 
        //Customerdict
        // int, customer

        //if customer is added, write on file 1 and save
        //press to read whole customerdict, read on file 1

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
