using System;
using System.Collections.Generic;
using System.Text;

namespace KiwiBankomaten
{
    internal abstract class User
    {
        public abstract string UserName { get; }
        public abstract string Password { get; }
        public abstract int Id { get; }

    }
}
