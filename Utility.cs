using System;

namespace KiwiBankomaten
{
    internal class Utility
    {
        public static bool CheckPassWord(int userKey)
        {
            //if the user is locked, message is displayed and user is returned to mainmenu

            for (int i = 4 - 1; i >= 0; i--)
            {
                CheckPassWordLimit(userKey, i);

                if (UserInterface.QuestionForString("Ange ditt Lösenord", "Lösenord: ") == DataBase.CustomerDict[userKey].Password)
                {
                    UserInterface.CurrentMethod("Rätt Lösenord. Du loggas nu in");
                    return true;
                }
                else
                {
                    if (!(i <= 1))
                    {
                        UserInterface.CurrentMethod($"Fel lösenord, du har nu {i - 1} försök kvar. Försök igen");
                        PressEnterToContinue();
                        RemoveLines(8);
                    }
                    else
                    {
                        CheckPassWordLimit(userKey, i);
                    }
                }
            }
            return false;
        }

        public static bool CheckAdminPassWord(int adminKey)
        {
            //if the user is locked, message is displayed and user is returned to mainmenu

            for (int i = 0; i < 3; i++)
            {
                if (UserInterface.PromptForString("Enter your password") == DataBase.AdminList[adminKey].Password)
                {
                    Console.WriteLine("Password is correct");
                    return true;
                }
                else
                {
                    Console.WriteLine("Wrong password"); //if wrong password is entered
                    PressEnterToContinue();
                    RemoveLines(5);
                } 
            }
            UserInterface.DisplayMessage("Du har inte lyckats logga in på 3 försök... fan va du suger på detta...");
            PressEnterToContinue();
            Program.RunProgram();
            return false;
        }

        public static void CheckPassWordLimit(int userKey, int tries)
        {
            //if the user is locked, message is displayed and user is returned to mainmenu
            if (tries <= 0 || DataBase.CustomerDict[userKey].Locked == true)
            {
                DataBase.CustomerDict[userKey].Locked = true;//locks user if 3 fails occur

                UserInterface.CurrentMethod("Du har angett fel lösenord 3 gånger, ditt konto är låst. Kontakta admin");

                PressEnterToContinue();

                Program.RunProgram();
            }
        }

        //Method to separete commas in amount
        public static string AmountDecimal(decimal valueDec)
        {
            //the 0 is a placeholder, which shows even if the value is 0 
            string stringDec = valueDec.ToString("#,##0.00");
            return stringDec;
        }

        // Stops the program until the user presses "Enter"
        public static void PressEnterToContinue()
        {
            UserInterface.DisplayMessage(" Klicka Enter för att fortsätta.");
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

            UserInterface.DisplayMessage(" Klicka Enter för att försöka igen eller Esc för" +
                " att återgå till huvudmenyn.");
            // Gets the input from the user
            ConsoleKey userInput = Console.ReadKey(true).Key;
            // Loops if the user Presses any button other than "Enter"
            while (userInput != ConsoleKey.Enter && userInput != ConsoleKey.Escape)
            {
                UserInterface.DisplayMessage("|Felaktig inmatning, välj Enter för att försöka " +
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

        public static bool YesOrNo(string input, string accountName)
        {
            switch (UserInterface.QuestionForString($"Ditt konto får {input} {accountName}\n" +
                $"Vill du godkänna detta? [J/N]").ToUpper())
            {
                case "J":
                    return false;
                case "N":
                    RemoveLines(7);
                    return true;
                default:
                    UserInterface.DisplayMessage("Felaktig inmatning\nVälj [J] " +
                        "för ja eller N för nej.");
                    PressEnterToContinue();
                    RemoveLines(10);
                    YesOrNo(input, accountName);
                    break;
            }
            return false;
        }

        public static void RemoveLines(int lines)
        {
            for (int i = 0; i < lines; i++)
            {
                Console.SetCursorPosition(0, Console.CursorTop - 1);
                Console.Write(new string(' ', Console.BufferWidth));
            }
            Console.SetCursorPosition(0, Console.CursorTop);
        }

        public static void MoveCursorTo(int value)
        {
            Console.SetCursorPosition(value, Console.CursorTop);
            Console.WriteLine("|");
        }
    }
}
