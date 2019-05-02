using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeepee.IO.Receiver.Application.Behaviours
{
    public class RequestLoggerBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        private readonly ILogger _logger;

        public RequestLoggerBehaviour(ILogger logger)
        {
            _logger = logger;
        }

        public Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            var name = typeof(TRequest).Name;
            _logger.Information("Request started {name}", name);

            return next();
        }
    }
}
