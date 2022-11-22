using System;
using System.Collections.Generic;
using System.Text;

namespace KiwiBankomaten
{
    internal class Account
    {
        private string _name;
        private decimal _value;
        private string _currency;
        private decimal _interest;
        private List<string> _log;

        public string Name {get => _name;}
        public decimal Value { get => _value; set => _value = value; }
        public string Currency { get => _currency; set => _currency = value; }
        public decimal Interest { get => _interest; set => _interest = value; }

        public Account()
        {

        }

        public Account(string _name, decimal _value, string _currency,
            decimal _interest)
        {
            this._name = _name;
            this._value = _value;
            this._currency = _currency;
            this._interest = _interest;
        }

        public Account(string _name, string _currency, decimal _interest)
        {
            this._name = _name;
            _value = 0;
            this._currency = _currency;
            this._interest = _interest;
        }
    }
}
