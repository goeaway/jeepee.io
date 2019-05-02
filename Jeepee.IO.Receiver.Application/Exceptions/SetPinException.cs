using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Exceptions
{
    public class SetPinException : ReceiverException
    {
        public SetPinException(string message, Exception innerException) : base(message, innerException) { }
    }
}
