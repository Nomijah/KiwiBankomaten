using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace KiwiBankomaten
{
    internal class Utility
    {
        // Checks whether or not "decimal amountMoney" is valid input
        public static void IsValueNumberCheck(out decimal amountMoney)
        {
            // Gets user input => Checks if it's a decimal => Checks if it's larger than 0
            while (!(decimal.TryParse(Console.ReadLine(), out amountMoney) && amountMoney > 0))
            {
                Console.WriteLine("Please input a positive number that is greater than 0.");
            }
        }

        // Checks whether or not "int transferFromOrToWhichAccount" is valid input
        public static void IsValueNumberCheck(out int transferFromOrToWhichAccount, int maxValue)
        {
            int minValue = 1;
            // Gets user input => Checks if it's an integer => Checks if it's in the set range
            while (!(int.TryParse(Console.ReadLine(), out transferFromOrToWhichAccount) && minValue <= transferFromOrToWhichAccount && maxValue >= transferFromOrToWhichAccount))
            {
                Console.WriteLine("Please input a Number that is within the specified range. \n[{0} - {1}]", minValue, maxValue);
            }
        }


        public static bool CheckPassWord(int userKey, int tries)
        {
            if (tries == 3) //Checks to see if user failed login trice, before requesting Password again
            {
                DataBase.CustomerDict[userKey].Locked = true;//locks user if 3 fails occur
            }
            //if the user is locked, message is displayed and user is returned to mainmenu
            if (DataBase.CustomerDict[userKey].Locked == true)
            {
                Console.WriteLine("Du har angett fel lösenord 3 gånger.\nDitt konto är låst\nKontakta admin ");
                Utility.PressEnterToContinue();
                Program.RunProgram();
                return false; //Returns to Login
            }
            Console.WriteLine("Enter your password");
            string userPassWord = (Console.ReadLine()); // lets user enter password

            //if the user is locked, message is displayed and user is returned to mainmenu
            if (userPassWord == DataBase.CustomerDict[userKey].Password)
            {
                Console.WriteLine("Password is correct");
                return true;
            }
            else
            {
                Console.WriteLine("Wrong password"); //if wrong password is entered 
                tries++;  //"tries"adds with one, and user is returned CheckPassWord again
                CheckPassWord(userKey, tries);
                return false;
            }
        }
        public static bool AdminCheckPassWord(int adminKey)
        {
            Console.WriteLine("Enter your password");
            string userPassWord = (Console.ReadLine());

            if (userPassWord == DataBase.AdminList[adminKey].Password)
            {
                Console.WriteLine("Password is correct");
                return true;
            }
            else
            {
                Console.WriteLine("Wrong password");
                return false;
            }
        }

        // Stops the program until the user presses "Enter"
        public static void PressEnterToContinue()
        {
            Console.WriteLine("Klicka Enter för att fortsätta.");
            // Gets the input from the user
            ConsoleKey enterPressed = Console.ReadKey(true).Key;
            // Loops if the user Presses any button other than "Enter"
            while (!Console.KeyAvailable && enterPressed != ConsoleKey.Enter)
            {
                enterPressed = Console.ReadKey(true).Key;
            }
        }

        // Choose to continue or go back to main menu.
        public static bool ContinueOrAbort()
        {

            Console.WriteLine("Klicka Enter för att försöka igen eller Esc för" +
                " att återgå till huvudmenyn.");
            // Gets the input from the user
            ConsoleKey userInput = Console.ReadKey(true).Key;
            // Loops if the user Presses any button other than "Enter"
            while (userInput != ConsoleKey.Enter && userInput != ConsoleKey.Escape)
            {
                Console.WriteLine("Felaktig inmatning, välj Enter för att försöka " +
                    "igen eller Esc för att återgå till huvudmenyn.");
                userInput = Console.ReadKey(true).Key;
            }
            if (userInput == ConsoleKey.Escape)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
