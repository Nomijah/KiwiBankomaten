using System;
using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;
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

            Console.Clear();

            //looping menu, which will run when the program is started 
            do
            {
                UserInterface.DisplayWelcomeMessage();
                UserInterface.DisplayMenu(new string[] {"Logga in", "Avsluta"});
                loggedIn = false;
                string choice = UserInterface.PromptForString();
                switch (choice)
                {
                    case "1":
                        LogIn(out loggedIn, out userKey); //pressing 1 leads to using Login()
                        if (loggedIn)
                        {
                            UserInterface.DisplayWelcomeMessageLoggedIn(userKey);
                            
                            Utility.PressEnterToContinue();
                            Console.Clear();

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
            } while (loggedIn != true);
        }
        
        //Checks if user exist, returns userKey
        public static void LogIn(out bool loggedIn, out int userKey)
        {
            userKey = 0; 
            int tries = 0;
            loggedIn = false;

            UserInterface.DisplayWelcomeMessage();

            do
            {
                string userName = UserInterface.PromptForString("Please enter your account name\n\nAccount Name").Trim();

                // loop through customer dictionary to search for userName
                foreach (KeyValuePair<int, Customer> item in DataBase.CustomerDict)
                {
                    if (userName == item.Value.UserName)
                    {
                        tries = 0;
                        userKey = item.Key;// stores userKey

                        loggedIn = Utility.CheckPassWord(userKey, tries); // calls CheckPassWord function to check password
                                                                          // if login is successful
                        if (loggedIn) { return; }
                    }

                }
                UserInterface.DisplayMessage("Användarnamnet som du försökte logga in med finns inte\nFörsök igen");
                Utility.PressEnterToContinue();
                Utility.RemoveLines(9); 
                tries++;
            } while (tries != 3);
            
            Console.WriteLine("Ingen användare med det namnet hittades.");
            return;
        }

        public static int AdminLogIn(out bool loggedIn)
        {
            int adminKey = 0;
            loggedIn = false;
            
            UserInterface.DisplayWelcomeMessage();
            string userName = UserInterface.PromptForString("Please enter your account name\n\nAccount Name");

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

                UserInterface.DisplayMessage($"{DataBase.CustomerDict[userKey].UserName}/CustomerMenu/");

                UserInterface.DisplayMenu(new string[] {"Overview accounts and balances", "Transfer money personal accounts",
                    "Create new account", "Kiwibank internal Transfer money ", "Logout"});

                string choice = UserInterface.PromptForString();

                switch (choice)
                {
                    case "1":
                        // Overviews the Accounts and their respective balances
                        Console.Clear();
                        UserInterface.DisplayMessage($"{DataBase.CustomerDict[userKey].UserName}/CustomerMenu/AccountOverview/");
                        obj.AccountOverview();
                        break;
                    case "2":
                        // Transfers a value between two accounts the user possesses
                        obj.TransferBetweenCustomerAccounts(); 
                        break;
                    case "3":
                        // Opens account for the specific user
                        obj.OpenAccount(); 
                        break;
                    case "4":
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
                Utility.PressEnterToContinue();

                // clearing console, 
                Console.Clear();

            } while (true);
        }
            

    }
}
