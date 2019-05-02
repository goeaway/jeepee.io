using FluentValidation.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Exceptions
{
    public class RequestValidationFailureException : ReceiverException
    {
        public IEnumerable<ValidationFailure> Failures { get; }

        public RequestValidationFailureException(IEnumerable<ValidationFailure> failures)
        {
            Failures = failures;
        }
    }
}
