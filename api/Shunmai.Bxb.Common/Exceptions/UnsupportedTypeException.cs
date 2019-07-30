using System;
using System.Collections.Generic;
using System.Text;

namespace Shunmai.Bxb.Common.Exceptions
{
    public class UnsupportedTypeException : Exception
    {
        private const string DEFAULT_MESSAGE = "Unsupported type.";

        public UnsupportedTypeException() : base(DEFAULT_MESSAGE)
        {

        }

        public UnsupportedTypeException(string parameterName, object type) : base($"{DEFAULT_MESSAGE} Argu_Name: {parameterName}, Type: {type}")
        {
            
        }
    }
}
