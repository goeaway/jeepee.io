using Jeepee.IO.Core.Models.DTOs;
using Jeepee.IO.Receiver.Application.Options;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Jeepee.IO.Receiver.Application.Queries
{
    public class GetChannelsHandler : IRequestHandler<GetChannels, IEnumerable<ChannelDTO>>
    {
        private readonly HardwareOptions _hardwareOptions;

        public GetChannelsHandler(HardwareOptions hardwareOptions)
        {
            _hardwareOptions = hardwareOptions;
        }

        public Task<IEnumerable<ChannelDTO>> Handle(GetChannels request, CancellationToken cancellationToken)
        {
            return Task.Run(() => _hardwareOptions.Channels.Select((c, i) => new ChannelDTO { Id = i, Name = c.Name }));
        }
    }
}
