using Jeepee.IO.Core.Exceptions;
using Jeepee.IO.Manager.Application.Abstractions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeepee.IO.Manager.Application.Commands
{
    public class ForwardControlHandler : IRequestHandler<ForwardControlRequest>
    {
        private readonly IJeepeeControlStore _jeepeeControlStore;
        private readonly IJeepeeInstanceInfoStore _infoStore;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;

        public ForwardControlHandler(IJeepeeInstanceInfoStore infoStore, IJeepeeControlStore jeepeeControlStore, ILogger logger)
        {
            _jeepeeControlStore = jeepeeControlStore;
            _logger = logger;
            _infoStore = infoStore;
        }

        public async Task<Unit> Handle(ForwardControlRequest request, CancellationToken cancellationToken)
        {
            var instance = _jeepeeControlStore.GetInstanceForUser(request.UserIdent);

            if (instance == null)
            {
                throw new JeepeeHTTPException("User not allowed to control jeepee instance", HttpStatusCode.Forbidden);
            }

            _logger.Information(
                "{0} forwarded control {1} {2} {3} to Jeepee instance {4}",
                request.UserIdent,
                request.DTO.Channel,
                request.DTO.Direction,
                request.DTO.On,
                instance);

            var client = new HttpClient();

            var content = new StringContent(
                JsonConvert.SerializeObject(request.DTO),
                Encoding.UTF8,
                "application/json");

            var info = _infoStore.GetInfoForId(instance);
            await client.PostAsync($"{info.URL}/channel/set", content);

            return Unit.Value;
        }
    }
}
