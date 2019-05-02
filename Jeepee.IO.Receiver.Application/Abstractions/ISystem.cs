using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Jeepee.IO.Receiver.Application.Abstractions
{
    public interface ISystem : IDisposable
    {
        List<IChannel> Channels { get; }
        Task SetPin(int pinNumber, bool on);
        Stream GetImage();
    }
}
