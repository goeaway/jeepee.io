using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Jeepee.IO.Core.Exceptions
{
    public class JeepeeException : Exception
    {
        public JeepeeException()
        {
        }

        public JeepeeException(string message) : base(message)
        {
        }

        public JeepeeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
