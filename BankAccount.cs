﻿using System;
using System.Collections.Generic;
using System.Text;

namespace KiwiBankomaten
{
    internal class BankAccount
    {
        private string _accountName;
        private decimal _amount;
        private string _currency;
        private decimal _interest;
        private List<string> _log;

        public string AccountName {get => _accountName;}
        public decimal Amount { get => _amount; set => _amount = value; }
        public string Currency { get => _currency; set => _currency = value; }
        public decimal Interest { get => _interest; set => _interest = value; }

        // For testing, to give value to accounts
        public BankAccount(string _accountName, decimal _value, string _currency,
            decimal _interest)
        {
            this._accountName = _accountName;
            this._amount = _value;
            this._currency = _currency;
            this._interest = _interest;
        }

        // Use this constructor for users
        public BankAccount(string _name, string _currency, decimal _interest)
        {
            this._accountName = _name;
            _amount = 0;
            this._currency = _currency;
            this._interest = _interest;
        }
    }
}