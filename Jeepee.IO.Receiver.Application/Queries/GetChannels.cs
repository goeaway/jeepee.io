using Jeepee.IO.Core.Models.DTOs;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Channels;

namespace Jeepee.IO.Receiver.Application.Queries
{
    public class GetChannels : IRequest<IEnumerable<ChannelDTO>>
    {

    }
}
