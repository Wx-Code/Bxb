using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Services.Exceptions
{
    public class IETLoginException : Exception
    {
        private const string DEFAUT_MESSAGE = "An exception occurs when trying to sign in on IET.";

        public IETLoginException() : base(DEFAUT_MESSAGE)
        {

        }

        public IETLoginException(string message) : base(message)
        {

        }
    }
}
