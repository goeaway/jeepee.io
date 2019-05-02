using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Abstractions
{
    public interface IChannel
    {
        int FirstPin { get; set; }
        int SecondPin { get; set; }
        int EnablePin { get; set; }
    }
}
