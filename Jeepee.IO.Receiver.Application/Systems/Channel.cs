using Jeepee.IO.Receiver.Application.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Systems
{
    public class Channel : IChannel
    {
        public int FirstPin { get; set; }
        public int SecondPin { get; set; }
        public int EnablePin { get; set; }
    }
}
