using Jeepee.IO.Receiver.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeepee.IO.Receiver.Application.Behaviours
{
    public class ExceptionWrapperBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception e)
            {
                throw new ReceiverException($"Exception occured during {typeof(TRequest).Name} request", e);
            }
        }
    }
}
