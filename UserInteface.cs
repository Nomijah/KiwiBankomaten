using KiwiBankomaten;
using System;

public class UserInterface
{
    // Display Logo
    public static void DisplayLogo()
    {
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("$$\\   $$\\$$\\             $$\\      ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("_____");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("       $$$$$$$\\                   $$\\       \r\n" +
                      "$$ | $$  \\__|            \\__|    ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("/     \\*");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("     $$  __$$\\                  $$ |      \r\n" +
                      "$$ |$$  /$$\\$$\\  $$\\  $$\\$$\\    ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("/  ._.  \\**");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("   $$ |  $$ |$$$$$$\\ $$$$$$$\\ $$ |  $$\\\r\n" +
                      "$$$$$  / $$ $$ | $$ | $$ $$ |  ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("|  ./ \\.  |**");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("  $$$$$$$\\ |\\____$$\\$$  __$$\\$$ | $$  |\r\n" +
                      "$$  $$<  $$ $$ | $$ | $$ $$ |  ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("|  .\\_/.  |**");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("  $$  __$$\\ $$$$$$$ $$ |  $$ $$$$$$  /\r\n" +
                      "$$ |\\$$\\ $$ $$ | $$ | $$ $$ |   ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("\\  ‘ ‘  /***");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("  $$ |  $$ $$  __$$ $$ |  $$ $$  _$$<  \r\n" +
                      "$$ | \\$$\\$$ \\$$$$$\\$$$$  $$ |    ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("\\_____/***");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("   $$$$$$$  \\$$$$$$$ $$ |  $$ $$ | \\$$\\\r\n" +
                      "\\__|  \\__\\__|\\_____\\____/\\__|      ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("******");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("     \\_______/ \\_______\\__|  \\__\\__|  \\__|\r\n");
        Console.ForegroundColor = ConsoleColor.White;
    }

    //Displays the Welcome message to the user
    public static void DisplayLogoMessage()
    {
        Console.Clear();
        DisplayLogo();
    }

    public static void DisplayAdminMessage()
    {
        Console.Clear();
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine("Admin Menyn! Super Secret!");
    }

    // Displays the Welcome message when user is logged in
    public static void DisplayWelcomeMessageLoggedIn(int userKey)
    {
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine("Välkommen: {0}", DataBase.CustomerDict[userKey].UserName);
    }

    public static void DisplayAdminMessageLoggedIn(int adminKey)
    {
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine("Välkommen Admin: {0}", DataBase.AdminList[adminKey].UserName);
    }

    // Displays a menu
    public static void DisplayMenu(string[] options)
    {
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine("Välj ett av följande alternativ:");
        for (int i = 0; i < options.Length; i++)
        {
            Console.WriteLine($"-{i + 1}) {options[i]}");
        }
    }

    // Checks whether or not "decimal selection" is valid input
    public static decimal IsValueNumberCheck()
    {
        Console.WriteLine("---------------------------------------------------");
        Console.Write("Ange ditt val: ");
        decimal selection;
        // Gets user input => Checks if it's a decimal => Checks if it's larger than 0
        while (!decimal.TryParse(Console.ReadLine(), out selection) || selection <= 0)
        {
            UserInterface.DisplayMessage("Ogiltigt inmatning. Inmatningen måste vara ett positivt nummer " +
                "\nVänligen försök igen.");
            Utility.PressEnterToContinue();
            Utility.RemoveLines(6);
            Console.Write("Ange ditt val: ");
        }
        return selection;
    }

    // Checks whether or not "int selection" is valid input
    public static int IsValueNumberCheck(int max)
    {
        Console.WriteLine("---------------------------------------------------");
        Console.Write("Ange ditt val: ");
        int selection;
        int min = 1;
        // Gets user input => Checks if it's an integer => Checks if it's in the set range
        while (!int.TryParse(Console.ReadLine(), out selection) || selection < min || selection > max)
        {
            UserInterface.DisplayMessage($"Ogiltigt inmatning. Inmatningen måste vara inom [{min} - {max}] " +
               "\nVänligen försök igen.");
            Utility.PressEnterToContinue();
            Utility.RemoveLines(6);
            Console.Write("Ange ditt val: ");
        }
        return selection;
    }

    // Checks whether or not "int selection" is valid input
    public static int IsValueNumberCheck(int max, string prompt)
    {
        Console.WriteLine("---------------------------------------------------");
        Console.Write($"{prompt}: ");
        int selection;
        int min = 1;
        // Gets user input => Checks if it's an integer => Checks if it's in the set range
        while (!int.TryParse(Console.ReadLine(), out selection) || selection < min || selection > max)
        {
            UserInterface.DisplayMessage($"Ogiltigt inmatning. Inmatningen måste vara inom [{min} - {max}] " +
                "\nVänligen försök igen.");
            Utility.PressEnterToContinue();
            Utility.RemoveLines(6);
            Console.Write($"{prompt}: ");
        }
        return selection;
    }

    // Asks for a value then returns that value as a string
    public static string PromptForString()
    {
        Console.WriteLine("---------------------------------------------------");
        Console.Write("Ange ditt val: ");
        return Console.ReadLine();
    }
    public static string PromptForString(out string value)
    {
        Console.WriteLine("---------------------------------------------------");
        Console.Write("Ange ditt val: ");
        value = Console.ReadLine();
        return value;
    }

    // Asks for a value with a custom prompt then returns that value as a string
    public static string PromptForString(string prompt)
    {
        Console.WriteLine("---------------------------------------------------");
        Console.Write($"{prompt}: ");
        return Console.ReadLine();
    }

    // Asks for a value with a custom prompt then returns that value as a string
    public static string PromptForString(string question, string prompt)
    {
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine($"{question}\n");
        Console.Write($"{prompt}: ");
        return Console.ReadLine();
    }
    // Asks for a value then returns that value as a string
    public static string QuestionForString(string question)
    {
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine($"{question}\n");
        Console.Write("Ange ditt val: ");
        return Console.ReadLine();
    }

    // Asks for a value then returns that value as a string
    public static string QuestionForString(string question, out string value)
    {
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine($"{question}\n");
        Console.Write("Ange ditt val: ");
        value = Console.ReadLine();
        return value;
    }


    // Asks for a value with a custom prompt then returns that value as a string
    public static int PromptForInt(out int value)
    {
        Console.WriteLine("---------------------------------------------------");
        Console.Write("Ange ditt val: ");
        value = (int)IsValueNumberCheck();
        return value;
    }

    // Replacement for Console.WriteLine()
    public static void DisplayMessage(string message)
    {
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine(message);
    }
    
    // Replacement for Console.WriteLine()
    public static void ClearDisplayMessage(string message)
    {
        Console.Clear();
        Console.WriteLine("---------------------------------------------------");
        Console.WriteLine(message);
    }
}