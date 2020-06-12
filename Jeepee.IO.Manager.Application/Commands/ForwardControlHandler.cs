using Jeepee.IO.Core.Exceptions;
using Jeepee.IO.Manager.Application.Abstractions;
using MediatR;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeepee.IO.Manager.Application.Commands
{
    public class ForwardControlHandler : IRequestHandler<ForwardControlRequest>
    {
        private readonly IJeepeeControlStore _jeepeeControlStore;
        private readonly ILogger _logger;

        public ForwardControlHandler(IJeepeeControlStore jeepeeControlStore, ILogger logger)
        {
            _jeepeeControlStore = jeepeeControlStore;
            _logger = logger;
        }

        public async Task<Unit> Handle(ForwardControlRequest request, CancellationToken cancellationToken)
        {
            var instanceInfo = _jeepeeControlStore.UserControl(request.UserIdent);

            if (instanceInfo == null)
            {
                throw new JeepeeHTTPException("User not allowed to control jeepee instance", HttpStatusCode.Forbidden);
            }

            _logger.Information(
                "{0} forwarded control {1} {2} {3} to Jeepee instance {4}",
                request.UserIdent,
                request.DTO.Channel,
                request.DTO.Direction,
                request.DTO.On,
                instanceInfo.Id);

            var client = new HttpClient();

            var content = new StringContent(
                JsonConvert.SerializeObject(request.DTO),
                Encoding.UTF8,
                "application/json");

            await client.PostAsync($"{instanceInfo.URL}/channel/set", content);

            return Unit.Value;
        }
    }
}
