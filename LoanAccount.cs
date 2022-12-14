using System;

namespace KiwiBankomaten
{
    internal class LoanAccount : BankAccount
    {

        private static int LoanNumberCounter = 41443033;

        // Use this constructor in program.
        public LoanAccount(string _accountName, decimal _value,decimal _interest)
        {
            AccountName = _accountName;
            Amount = _value;
            Currency = "SEK";
            Interest = _interest;
            AccountNumber = LoanNumberCounter;
            LoanNumberCounter++;
        }

        // Use this constructor for DataSaver.UpdateFromFile().
        public LoanAccount(int _accountNumber, string _accountName, decimal _amount, 
            string _currency, decimal _interest)
        {
            AccountName = _accountName;
            Amount = _amount;
            Currency = _currency;
            Interest = _interest;
            AccountNumber = _accountNumber;
        }

        public void DownPayment()
        {

        }
    }
}
