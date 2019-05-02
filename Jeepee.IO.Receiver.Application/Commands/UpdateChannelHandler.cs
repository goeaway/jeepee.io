using Jeepee.IO.Receiver.Application.Abstractions;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Commands
{
    public class UpdateChannelHandler : RequestHandler<UpdateChannel>
    {
        private readonly ISystem _system;
        private readonly ILogger _logger;

        public UpdateChannelHandler(ISystem system, ILogger logger)
        {
            _system = system;
            _logger = logger;
        }

        protected override void Handle(UpdateChannel request)
        {
            _logger.Information("Update channel request started CH: {Channel}, D: {Direction}, O: {On}", request.Channel, request.Direction, request.On);
            var channel = _system.Channels[request.Channel];

            _system.SetPin(channel.EnablePin, request.On);
            _system.SetPin(channel.FirstPin, request.Direction);
            _system.SetPin(channel.SecondPin, !request.Direction);
        }
    }
}
