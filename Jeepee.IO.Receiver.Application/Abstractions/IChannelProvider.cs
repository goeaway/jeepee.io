using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Abstractions
{
    public interface IChannelProvider
    {
        IEnumerable<IChannel> GetChannels();
    }
}
