﻿using System;
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
        private List<string> _log;
        
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
            AccountNumberCounter++;
        }

        public BankAccount()
        {

        }


    }
}
