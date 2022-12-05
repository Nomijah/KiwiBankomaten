using System;
using System.Collections.Generic;
using System.Threading;

namespace KiwiBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {
            RunProgram();
        }
        public static void RunProgram()
        {
            bool loggedIn;
            int userKey = 0;
            int adminKey = -1;
                
            //looping menu, which will run when the program is started 
            do 
            {
                loggedIn = false;
                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine("Välj ett av alternativen nedan:");
                Console.WriteLine("-1) Logga in\n-2) Stäng av");
                string choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        LogIn(out loggedIn, out userKey); //pressing 1 leads to using Login()
                        if (loggedIn)
                        {
                            CustomerMenu(userKey);//if login is successful, leads to CustomerMenu() 
                        }
                        break;
                    case "2"://Exit program
                        Environment.Exit(0);//closes the program 
                        break;
                    case "admin": // Hidden admin login, which leads to AdminLogIn() and AdminMenu()
                        adminKey = AdminLogIn(out loggedIn);
                        if (loggedIn)
                        {
                            Admin.AdminMenu();
                        }
                        break;
                    default://If neither of these options are used the defaultmsg is displayed
                        Console.WriteLine("Felaktigt val, försök igen.");
                        Utility.PressEnterToContinue();
                        Console.Clear();
                        RunProgram();
                        break;
                }
                Thread.Sleep(2000);//leaves eventual message readable for 2 sec
                Console.Clear();// clearing console, 
            } while (loggedIn != true);
        }
        
        //Checks if user exist, returns userKey
        public static void LogIn(out bool loggedIn, out int userKey)
        {
            userKey = 0; 
            int tries = 0;
            loggedIn = false;

            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Welcome to KiwiBank");
            Console.WriteLine("Please enter your account name:");
            string userName = Console.ReadLine();

            // loop through customer dictionary to search for userName
            foreach (KeyValuePair<int, Customer> item in DataBase.CustomerDict)
            {
                if (userName == item.Value.UserName)
                {
                    userKey = item.Key;// stores userKey
                    loggedIn = Utility.CheckPassWord(userKey, tries); // calls CheckPassWord function to check password
                    loggedIn = true; //defines bool since it will change otherwise, dont change!

                    // if login is successful
                    if (loggedIn)
                    {
                        // clearing console, 
                        Console.Clear();
                        
                        return;
                    }
                }
                else
                {//if name is not found, bool is false, and the WriteLine below is shown once after the try
                    loggedIn = false;
                }
            }
            Console.WriteLine("Ingen användare med det namnet hittades.");
            return;
        }

        public static int AdminLogIn(out bool loggedIn)
        {
            int adminKey = 0;
            loggedIn = false;
            Console.WriteLine("---------------------------------------------------");
            Console.WriteLine("Welcome to KiwiBank");
            Console.WriteLine("Please enter your account name:");
            string userName = Console.ReadLine();

            foreach (Admin item in DataBase.AdminList)
            {
                if (userName == item.UserName)
                {
                    adminKey = DataBase.AdminList.FindIndex(item => userName == item.UserName);

                    loggedIn = Utility.AdminCheckPassWord(adminKey);

                    if (loggedIn)
                    {
                        Console.WriteLine("Successfully logged in");
                        Console.WriteLine($"Welcome {userName}");
                        return adminKey; //Returns adminKey so we know which admin is logged in
                    }
                }
            }
            return 0;
        }

        public static void LogOut()
        {
            // Makes the program go back to the log in menu
            RunProgram(); 
        }

        public static void CustomerMenu(int userKey)
        {
            // Looping menu  
            do 
            {
                // Creates an instance of the loggedIn user in database
                Customer obj = DataBase.CustomerDict[userKey];

                Console.WriteLine("---------------------------------------------------");
                Console.WriteLine($"Welcome {obj.UserName}");
                Console.WriteLine("---------------------------------------------------");

                Console.WriteLine("Enter a number as input to navigate in the menu:");
                Console.WriteLine("-1) Overview accounts and balances\n-2) Transfer money personal accounts" +
                    "\n-3) Create new account \n-4) Kiwibank internal Transfer money \n-5) Logout");
                
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine("---------------------------------------------------");
                        // Overviews the Accounts and their respective balances
                        obj.AccountOverview();
                        break;
                    case "2":
                        Console.WriteLine("---------------------------------------------------");
                        // Transfers a value between two accounts the user possesses
                        obj.TransferBetweenCustomerAccounts(); 
                        break;
                    case "3":
                        // Opens account for the specific user
                        obj.OpenAccount(); 
                        break;
                    case "4":
                        Console.WriteLine("---------------------------------------------------");
                        // Transfer money to other user in bank
                        obj.InternalMoneyTransfer(); 
                        break;
                    case "5":
                        // Logout
                        LogOut(); 

                        break;

                    default:
                        Console.WriteLine("Wrong input, enter available choice only!");
                        break;
                }
                Console.WriteLine("---------------------------------------------------");
                Utility.PressEnterToContinue();

                // clearing console, 
                Console.Clear();

            } while (true);
        }
            

    }
}
