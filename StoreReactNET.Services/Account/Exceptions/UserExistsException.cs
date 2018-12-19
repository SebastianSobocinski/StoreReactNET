using System;
using System.Collections.Generic;
using System.Text;

namespace StoreReactNET.Services.Account.Exceptions
{
    public class UserExistsException : Exception
    {
        public UserExistsException(string message) : base(message)
        {
        }
    }
}
