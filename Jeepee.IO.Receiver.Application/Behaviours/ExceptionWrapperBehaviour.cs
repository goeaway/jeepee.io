using Jeepee.IO.Receiver.Application.Exceptions;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Jeepee.IO.Receiver.Application.Behaviours
{
    public class ExceptionWrapperBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public ExceptionWrapperBehaviour(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            try
            {
                return await next();
            }
            catch (Exception e)
            {
                var requestName = typeof(TRequest).Name;
                _logger.Error(e, "Error occured when processing request {requestName}", requestName);
                throw new ReceiverException($"Exception occured during {requestName} request", e);
            }
        }
    }
}
