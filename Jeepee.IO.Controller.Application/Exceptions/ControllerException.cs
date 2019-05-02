using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Controller.Application.Exceptions
{
    public class ControllerException : Exception
    {
        public ControllerException() { }
        public ControllerException(string message) : base(message) { }
        public ControllerException(string message, Exception innerException) : base(message, innerException) { }
    }
}
