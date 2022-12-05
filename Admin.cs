using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace KiwiBankomaten
{
    internal class Admin : User
    {
        // Used for creating test admins.
        public Admin(int id, string username, string password)
        {
            Id = id;
            UserName = username;
            Password = password;
        }
        // Use this when creating admins in program.
        public Admin(string username, string password)
        {
            if (DataBase.AdminList == null)
            {
                Id = 1;
            }
            else
            {
                int newId = DataBase.AdminList.Count;
                Id = newId;
            }
            UserName = username;
            Password = password;
            IsAdmin = true;
        }
        public static void CreateNewUser()
        {
            bool error;
            do
            {
                error = false;
                Console.Clear();
                Console.WriteLine("Vilken sorts användare vill du skapa?\n-1 Customer\n-2 Admin");
                string userType = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Vilket användarnamn ska den nya användaren ha?");
                string userName = Console.ReadLine();
                Console.Clear();
                Console.WriteLine("Vilket lösenord ska den nya användaren ha?");
                string passWord = Console.ReadLine();
                Console.Clear();

                switch (userType)
                {
                    case "1":
                        DataBase.CustomerDict.Add(DataBase.CustomerDict.Last().Key + 1, new Customer(userName, passWord));
                        Console.WriteLine($"Customer {userName} har skapats med nyckeln {DataBase.CustomerDict.Last().Key}");
                        break;
                    case "2":
                        DataBase.AdminList.Add(new Admin(userName, passWord));
                        Console.WriteLine($"Admin {userName} har skapats.");
                        break;
                    default:
                        Console.WriteLine("Fel input, skriv in ett korrekt värde");
                        error = true;
                        break;
                }
            } while (error == true);
        }
        public static void AdminMenu()
        {
            bool loggedIn = true;
            while (loggedIn == true)
            {
                Console.Clear();
                Console.WriteLine("Funktioner för admins:\n-1 Skapa ny användare\n-2 Uppdatera växlingskurs" +
                    "\n-3 Välj en specifik användare\n-4 Redigera ett användarkonto\n-5 Logga ut");
                switch (Console.ReadLine())
                {
                    case "1":
                        CreateNewUser();
                        break;
                    case "2": //UpdateExchangeRate();
                        break;
                    case "3": Console.Clear(); 
                        ViewAllUsers();
                        Console.ReadKey();
                        break;
                    case "4": Console.Clear();
                        EditUserAccount();
                        break;
                    case "5": loggedIn = false;
                        Console.Clear();
                        break;
                    default:
                        Console.WriteLine("Fel input, skriv in ett korrekt värde");
                        break;
                }
            }
            Program.LogOut();
        }
        // Method for printing out all customer accounts with their ID, Username, Password and IsLocked status.
        public static void ViewAllUsers()
        {
            Console.WriteLine("---Alla användarkonton i Kiwibank---");
            foreach (KeyValuePair<int, Customer> customer in DataBase.CustomerDict)
            {
                Console.WriteLine($"ID:{customer.Value.Id} - Användarnamn:{customer.Value.UserName} - " +
                    $"Lösenord:{customer.Value.Password} - Spärrat:{customer.Value.Locked}");
            }
        }
        // Method for selecting one specific user account and then giving the admin a choice of what to do
        // with the account.
        public static void EditUserAccount()
        {
            bool noError;
            do
            {
                Console.Clear();
                // Prints all user accounts so admin can see which accounts they can select.
                ViewAllUsers();
                Console.WriteLine("Vilken användare vill du välja? Välj genom att skriva in ID");
                // Admin inputs user ID here to select which account they want to edit.
                noError = Int32.TryParse(Console.ReadLine(), out int userID);
                // Checks if admin input is a number and if that number exists as a key in the customer dictionary.
                if (noError && DataBase.CustomerDict.ContainsKey(userID))
                {
                    // Chosen customer account is saved for access to non-static methods and easier property access.
                    Customer selectedUser = DataBase.CustomerDict[userID];
                    do
                    {
                        Console.Clear();
                        Console.WriteLine($"Du har valt användaren ID:{selectedUser.Id} - Användarnamn:{selectedUser.UserName}" +
                            $" - Lösenord:{selectedUser.Password} - Spärrad:{selectedUser.Locked}");
                        Console.WriteLine("Vad vill du göra med användaren?\n-1 Visa bankkonton\n-2 Lås/Lås upp" +
                            "\n-3 Ändra lösenord\n-4 Återvänd till adminmenyn");
                        string userChoice = Console.ReadLine();
                        switch (userChoice)
                        {
                            // Prints all user's bank accounts.
                            case "1":
                                selectedUser.AccountOverview();
                                Console.ReadKey();
                                break;
                            // Lets admin lock or unlock user's account.
                            case "2":
                                LockOrUnlockAccount(selectedUser);
                                Console.ReadKey();
                                break;
                            // Lets admin change password of user's account.
                            case "3":
                                ChangeUserPassword(selectedUser);
                                break;
                            // Returns admin to the main admin menu.
                            case "4":
                                return;
                            default: 
                                Console.WriteLine("Fel input, skriv in ett korrekt värde");
                                break;
                        }
                    } while (noError == false);
                }
                else
                {
                    Console.WriteLine("Ogiltig användare vald, skriv in ett giltigt ID");
                    noError = false;
                }
            } while (noError == false);
        }
        // Method for locking user account if it is not locked, or unlocking user account if it is locked.
        public static void LockOrUnlockAccount(Customer selectedUser)
        {
            string answer;
            // Checks if user account is locked or not, asks different question depending on status.
            if (selectedUser.Locked)
            {
                Console.WriteLine("Kontot är spärrat");
                Console.WriteLine("Vill du avspärra kontot? J/N");
            }
            else
            {
                Console.WriteLine("Kontot är inte spärrat");
                Console.WriteLine("Vill du spärra kontot? J/N");
            }
            do
            {
                answer = Console.ReadLine().ToUpper();
                // If admin confirms that they want to lock/unlock user then we do so.
                switch (answer)
                {
                    case "J":
                        if (selectedUser.Locked)
                        {
                            selectedUser.Locked = false;
                        }
                        else
                        {
                            selectedUser.Locked = true;
                        }
                        break;
                    case "N":
                        break;
                    default:
                        Console.WriteLine("Fel input, välj antingen J/N");
                        break;
                }
            } while (answer != "J" && answer != "N");

        }
        // Method for changing user account's password.
        public static void ChangeUserPassword(Customer selectedUser)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"Vill du ändra lösenordet till användaren {selectedUser.Id} - {selectedUser.UserName}? J/N");
                if (Console.ReadLine().ToUpper() == "J")
                {
                    Console.Clear();
                    Console.WriteLine($"Skriv in det nya lösenordet till användaren {selectedUser.UserName}");
                    string newPassWord = Console.ReadLine();
                    Console.WriteLine("Konfirmera användarens lösenord genom att skriva in det igen");
                    // Makes user input the password again to confirm that it is what they want.
                    if (Console.ReadLine() == newPassWord)
                    {
                        selectedUser.Password = newPassWord;
                        return;
                    }
                    else
                    {
                        Console.WriteLine("Du konfirmerade inte lösenordet");
                        Console.ReadKey();
                    }
                }
                else { return; }
            }

        }
    }
}
