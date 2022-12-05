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

            bool isValueNumberCheck = false;

            do
            {
                // Gets user input => Checks if it's a decimal => Checks if it's larger than 0
                if (decimal.TryParse(Console.ReadLine(), out amountMoney) && amountMoney > 0)
                {
                    isValueNumberCheck = false; // If amountMoney is a valid input
                }
                else if (amountMoney < 0) // Checks if amountMoney is larger than 0
                {
                    Console.WriteLine("Input has to be positive and cannot be 0. Please Try Again!");
                    isValueNumberCheck = true; // If amountMoney is lower or equal to 0
                }
                else // If the answer is not a number or other invalid input
                {
                    Console.WriteLine("Please input a Number!");
                    isValueNumberCheck = true; // If amountMoney is an invalid input
                }

            } while (isValueNumberCheck);
        }

        // Checks whether or not "int transferFromOrToWhichAccount" is valid input
        public static void IsValueNumberCheck(out int transferFromOrToWhichAccount, int maxValue)
        {
            bool isValueNumberCheck = false;
            int minValue = 1;

            do
            {
                // Gets user input => Checks if it's a decimal => Checks if it's in the set range
                if (int.TryParse(Console.ReadLine(), out transferFromOrToWhichAccount) && minValue <= transferFromOrToWhichAccount && maxValue >= transferFromOrToWhichAccount)
                {
                    isValueNumberCheck = false; // If transferFromOrToWhichAccount is a valid input
                }
                else if (minValue > transferFromOrToWhichAccount || maxValue < transferFromOrToWhichAccount) // Checks if transferFromOrToWhichAccount is in the given range 
                {
                    Console.WriteLine("Please input a Number between {0} and {1}!", minValue, maxValue);
                    isValueNumberCheck = true; // If transferFromOrToWhichAccount is an invalid input
                }
                else // If the answer is not a number or other invalid input
                {
                    Console.WriteLine("Please input a Number!");
                    isValueNumberCheck = true; // If transferFromOrToWhichAccount is an invalid input
                }

            } while (isValueNumberCheck); // Loops if input is invalid
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
                Thread.Sleep(3000);//is shown message and returns to the menu.
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
