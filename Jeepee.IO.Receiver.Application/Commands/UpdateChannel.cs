using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Commands
{
    public class UpdateChannel : IRequest
    {
        public int Channel { get; set; }
        public bool Direction { get; set; }
        public bool On { get; set; }

        public UpdateChannel(int channel, bool direction, bool on)
        {
            Channel = channel;
            Direction = direction;
            On = on;
        }
    }
}
