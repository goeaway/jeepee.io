using MediatR;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Pippin.Core;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeepee.IO.Receiver.Application.Commands
{
    public class UpdateChannelHandler : IRequestHandler<UpdateChannel>
    {
        private readonly ILogger _logger;
        private readonly IPinSetter _pinSetter;
        private readonly IConfiguration _configuration;

        public UpdateChannelHandler(ILogger logger, IPinSetter pinSetter, IConfiguration configuration)
        {
            _logger = logger;
            _pinSetter = pinSetter;
            _configuration = configuration;
        }

        public async Task<Unit> Handle(UpdateChannel request, CancellationToken cancellationToken)
        {
            _logger.Information("Update channel request started CH: {Channel}, D: {Direction}, O: {On}", request.Channel, request.Direction, request.On);

            var channel = _configuration.GetSection($"Channels:{request.Channel}");
            await _pinSetter.SetPinAsync(Convert.ToInt32(channel["e"]), request.On);
            await _pinSetter.SetPinAsync(Convert.ToInt32(channel["1"]), request.Direction);
            await _pinSetter.SetPinAsync(Convert.ToInt32(channel["2"]), !request.Direction);

            return Unit.Value;
        }
    }
}
