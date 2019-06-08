using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Exceptions
{
    public class RTMPStreamException : ReceiverException
    {
        public RTMPStreamException() { }
        public RTMPStreamException(string message) : base(message) { }
        public RTMPStreamException(string message, Exception innerException) : base(message, innerException) { }
    }
}
