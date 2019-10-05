using System;
using System.Collections.Generic;
using System.Text;
using Jeepee.IO.Receiver.Application.Abstractions;
using Jeepee.IO.Receiver.Application.Systems;

namespace Jeepee.IO.Receiver.Application.Models
{
    public class ChannelsConfig
    {
        public IEnumerable<Channel> Channels { get; set; }
    }
}
