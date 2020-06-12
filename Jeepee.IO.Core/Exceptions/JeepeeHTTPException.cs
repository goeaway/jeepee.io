using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace Jeepee.IO.Core.Exceptions
{
    public class JeepeeHTTPException : JeepeeException
    {
        public HttpStatusCode StatusCode { get; }
            = HttpStatusCode.InternalServerError;

        public JeepeeHTTPException()
        {
        }

        public JeepeeHTTPException(string message, HttpStatusCode statusCode) : base (message)
        {
            StatusCode = statusCode;
        }

        public JeepeeHTTPException(string message, HttpStatusCode statusCode, Exception innerException) : base(message, innerException)
        {
            StatusCode = statusCode;
        }

        public JeepeeHTTPException(string message) : base(message)
        {
        }

        public JeepeeHTTPException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
