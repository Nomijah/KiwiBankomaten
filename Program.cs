using System;

namespace KiwiBankomaten
{
    internal class Program
    {
        static void Main(string[] args)
        {

        }

        public void LogIn()
        {

            Console.WriteLine("Welcome to KiwiBank");
            Console.WriteLine("Please enter your account name:");
            int userName = int.Parse(Console.ReadLine());
            int pinCode = int.Parse(Console.ReadLine());


        }
    }
}
