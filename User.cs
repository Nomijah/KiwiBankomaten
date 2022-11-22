using System;
using System.Collections.Generic;
using System.Text;

namespace KiwiBankomaten
{
    internal abstract class User
    {

        private string _userName;
        private string _password;
        private int _id;

        public string UserName
        { 
            get 
            { 
                return this._userName;
            }
            set
            {
                this._userName = value;
            }
        }
        public string Password
        {
            get
            {
                return this._password;
            }
            set
            {
                this._password = value;
            }
        }
        public int Id
        {
            get
            {
                return this._id;
            }
            set
            {
                this._id = value;
            }
        }
    }



}
