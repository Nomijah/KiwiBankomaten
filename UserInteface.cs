using KiwiBankomaten;
using System;

public class UserInterface
{
    // Display Logo
    public static void DisplayLogo()
    {
        Console.Write(" |");
        
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("$$\\   $$\\$$\\             $$\\      ");
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("_____");
        
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("       $$$$$$$\\                   $$\\       ");
        
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("|\r\n |");
        



        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("$$ | $$  \\__|            \\__|    ");
        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("/     \\*");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("     $$  __$$\\                  $$ |      ");
        
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("|\r\n |");
        



        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("$$ |$$  /$$\\$$\\  $$\\  $$\\$$\\    ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("/  ._.  \\**");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("   $$ |  $$ |$$$$$$\\ $$$$$$$\\ $$ |  $$\\ ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("|\r\n |");




        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("$$$$$  / $$ $$ | $$ | $$ $$ |  ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("|  ./ \\.  |**");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("  $$$$$$$\\ |\\____$$\\$$  __$$\\$$ | $$  |");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("|\r\n |");




        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("$$  $$<  $$ $$ | $$ | $$ $$ |  ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("|  .\\_/.  |**");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("  $$  __$$\\ $$$$$$$ $$ |  $$ $$$$$$  / ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("|\r\n |");




        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("$$ |\\$$\\ $$ $$ | $$ | $$ $$ |   ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("\\  ‘ ‘  /***");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("  $$ |  $$ $$  __$$ $$ |  $$ $$  _$$<  ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("|\r\n |");




        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("$$ | \\$$\\$$ \\$$$$$\\$$$$  $$ |    ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("\\_____/***");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("   $$$$$$$  \\$$$$$$$ $$ |  $$ $$ | \\$$\\ ");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("|\r\n |");




        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("\\__|  \\__\\__|\\_____\\____/\\__|      ");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write("******");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("     \\_______/ \\_______\\__|  \\__\\__|  \\__|");
        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("|\r\n");



        Console.ForegroundColor = ConsoleColor.White;
    }

    //Displays the Welcome message to the user
    public static void DisplayLogoMessage()
    {
        Console.Clear();
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        DisplayLogo();
    }

    public static void CurrentMethod(string message)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{message}");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
    }

    public static void CurrentMethodMagenta(string message)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write($"{message}");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
    }

    public static void CurrentMethodRed(string message)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{message}");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
    }
    public static void CurrentMethodGreen(string message)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.DarkGreen;
        Console.Write($"{message}");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
    }

    public static void CurrentMethodRed(string message, string message2)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{message}");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.Write($"{message2}");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
    }
    
    public static void DisplayAdminMessage()
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Admin Menyn! Super Secret!");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
    }

    // Displays the Welcome message when user is logged in
    public static void DisplayWelcomeMessageLoggedIn(int userKey)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.Write("Välkommen: {0}", DataBase.CustomerDict[userKey].UserName);
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
    }

    public static void DisplayAdminMessageLoggedIn(int adminKey)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.Write("Välkommen Admin: {0}", DataBase.AdminList[adminKey].UserName);
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
    }

    // Displays a menu
    public static void DisplayMenu(string[] options)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write("Välj ett av följande alternativ:");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
        for (int i = 0; i < options.Length; i++)
        {
            Console.Write(" |");
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.Write($"-{i + 1}) {options[i]}");
            Console.ForegroundColor = ConsoleColor.White;
            Utility.MoveCursorTo(85);
        }

    }

    // Checks whether or not "decimal selection" is valid input
    public static decimal IsValueNumberCheck()
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
         Console.Write("  Ange ditt val: ");
        decimal selection;
        // Gets user input => Checks if it's a decimal => Checks if it's larger than 0
        while (!decimal.TryParse(Console.ReadLine(), out selection) || selection <= 0)
        {
            UserInterface.CurrentMethodRed("Ogiltigt inmatning. Inmatningen måste vara ett positivt nummer, " +
                "försök igen.");
            Utility.PressEnterToContinue();
            Utility.RemoveLines(5);
             Console.Write("  Ange ditt val: ");
        }
        return selection;
    }

    // Checks whether or not "int selection" is valid input
    public static int IsValueNumberCheck(int max)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
         Console.Write("  Ange ditt val: ");
        int selection;
        int min = 1;
        // Gets user input => Checks if it's an integer => Checks if it's in the set range
        while (!int.TryParse(Console.ReadLine(), out selection) || selection < min || selection > max)
        {
            UserInterface.CurrentMethodRed($"Ogiltigt inmatning. Inmatningen måste vara inom [{min} - {max}], " +
               "försök igen.");
            Utility.PressEnterToContinue();
            Utility.RemoveLines(5);
             Console.Write("  Ange ditt val: ");
        }
        return selection;
    }

    // Checks whether or not "int selection" is valid input
    public static int IsValueNumberCheck(int max, string prompt)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write($"  {prompt}: ");
        int selection;
        int min = 1;
        // Gets user input => Checks if it's an integer => Checks if it's in the set range
        while (!int.TryParse(Console.ReadLine(), out selection) || selection < min || selection > max)
        {
            UserInterface.CurrentMethodRed($"Ogiltigt inmatning. Inmatningen måste vara inom [{min} - {max}], " +
                "försök igen.");
            Utility.PressEnterToContinue();
            Utility.RemoveLines(5);
            Console.Write($"  {prompt}: ");
        }
        return selection;
    }

    // Asks for a value then returns that value as a string
    public static string PromptForString()
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
         Console.Write("  Ange ditt val: ");
        string value = Console.ReadLine();
        return value;
    }
    public static string PromptForString(out string value)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
         Console.Write("  Ange ditt val: ");
        value = Console.ReadLine();
        return value;
    }

    // Asks for a value with a custom prompt then returns that value as a string
    public static string PromptForString(string prompt)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write($"  {prompt}: ");
        string value = Console.ReadLine();
        return value;
    }

    // Asks for a value then returns that value as a string
    public static string QuestionForString(string question)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{question}");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
         Console.Write("  Ange ditt val: ");
        string value = Console.ReadLine();
        return value;
    }
    
    // Asks for a value with a custom prompt then returns that value as a string
    public static string QuestionForString(string question, string prompt)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{question}");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write($"  {prompt}: ");
        string value = Console.ReadLine();
        return value;
    }

    // Asks for a value then returns that value as a string
    public static string QuestionForString(string question, out string value)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{question}");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
         Console.Write("  Ange ditt val: ");
        value = Console.ReadLine();
        return value;
    }


    // Asks for a value with a custom prompt then returns that value as a string
    public static int PromptForInt(out int value)
    {
        value = (int)IsValueNumberCheck();
        return value;
    }

    // Asks for a value with a custom prompt then returns that value as a string
    public static decimal QuestionForDecimal(string question, out decimal value)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{question}");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);

        value = IsValueNumberCheck();
        return value;
    }

    // Asks for a value with a custom prompt then returns that value as a string
    public static int QuestionForIntMax(string question, out int value, int max )
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.Write(" |");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.Write($"{question}");
        Console.ForegroundColor = ConsoleColor.White;
        Utility.MoveCursorTo(85);
        
        value = IsValueNumberCheck(max);
        return value;
    }

    // Replacement for Console.WriteLine()
    public static void DisplayMessage(string message)
    {
        Console.WriteLine(" +-----------------------------------------------------------------------------------+");
        Console.WriteLine($" {message}");
    }
    
    // Replacement for Console.WriteLine()
    public static void ClearDisplayMessage(string message)
    {
        Console.Clear();
        Console.WriteLine("+-----------------------------------------------------------------------------------+");
        Console.WriteLine($" {message}");
    }
}