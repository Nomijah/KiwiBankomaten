using System;
using System.Collections.Generic;

namespace KiwiBankomaten
{
    internal class BankAccount
    {
        private int _accountNumber;
        private string _accountName;
        private decimal _amount;
        private string _currency;
        private decimal _interest;
        //public List<Log> LogList;
        
        public int AccountNumber { get => _accountNumber; set => _accountNumber = value; }
        public string AccountName { get => _accountName; set => _accountName = value; }
        public decimal Amount { get => _amount; set => _amount = value; }
        public string Currency { get => _currency; set => _currency = value; }
        public decimal Interest { get => _interest; set => _interest = value; }
        
        private static int AccountNumberCounter = 40448653;
        
        // For testing, to give value to accounts.
        public BankAccount(string _accountName, decimal _value, string _currency,
            decimal _interest)
        {
            this._accountName = _accountName;
            this._amount = _value;
            this._currency = _currency;
            this._interest = _interest;
            _accountNumber = AccountNumberCounter;
            //LogList = new List<Log>();
            AccountNumberCounter++;
        }

        // Use this constructor for users.
        public BankAccount(string _name, string _currency, decimal _interest)
        {
            this._accountName = _name;
            _amount = 0;
            this._currency = _currency;
            this._interest = _interest;
            _accountNumber = AccountNumberCounter;
            //LogList = new List<Log>();
            AccountNumberCounter++;
        }

        // Use this constructor for DataSaver.UpdateFromFile.
        public BankAccount(int _accountNumber, string _name, decimal _amount,
            string _currency, decimal _interest)
        {
            this._accountName = _name;
            this._amount = _amount;
            this._currency = _currency;
            this._interest = _interest;
            this._accountNumber = _accountNumber;
        }

        public BankAccount()
        {

        }


    }
}
