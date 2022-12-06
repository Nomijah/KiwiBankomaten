using System;
using System.Collections.Generic;
using System.Text;

namespace KiwiBankomaten
{
    internal class LoanAccount : BankAccount
    {

        private static int LoanNumberCounter = 41443033;

        public LoanAccount(string _accountName, decimal _value,decimal _interest)
        {
            AccountName = _accountName;
            Amount = _value;
            Currency = "SEK";
            Interest = _interest;
            AccountNumber = LoanNumberCounter;
            LoanNumberCounter++;
        }

        public void DownPayment()
        {


        }

        


    }
}
