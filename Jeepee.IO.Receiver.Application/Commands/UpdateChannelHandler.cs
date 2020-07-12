using Jeepee.IO.Receiver.Application.Options;
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
        private readonly HardwareOptions _hardwareOptions;

        public UpdateChannelHandler(ILogger logger, IPinSetter pinSetter, HardwareOptions hardwareOptions)
        {
            _logger = logger;
            _pinSetter = pinSetter;
            _hardwareOptions = hardwareOptions;
        }

        public async Task<Unit> Handle(UpdateChannel request, CancellationToken cancellationToken)
        {
            _logger.Information("Update channel request started CH: {Channel}, D: {Direction}, O: {On}", request.Channel, request.Direction, request.On);

            var channel = _hardwareOptions.Channels[request.Channel];
            await _pinSetter.SetPinAsync(channel.Enable, request.On);
            await _pinSetter.SetPinAsync(channel.One, request.Direction);
            await _pinSetter.SetPinAsync(channel.Two, !request.Direction);

            return Unit.Value;
        }
    }
}
