using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Exceptions
{
    public class ReceiverException : Exception
    {
        public ReceiverException() { }
        public ReceiverException(string message) : base(message) { }
        public ReceiverException(string message, Exception innerException) : base(message, innerException) { }
    }
}
