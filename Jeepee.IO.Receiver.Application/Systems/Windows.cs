using Jeepee.IO.Receiver.Application.Abstractions;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Jeepee.IO.Receiver.Application.Systems
{
    public class Windows : ISystem
    {
        private readonly ILogger _logger;

        public Windows(ILogger logger)
        {
            Channels = new List<IChannel>
            {
                new Channel { EnablePin = 18, FirstPin = 24, SecondPin = 23 },
                new Channel { EnablePin = 16, FirstPin = 21, SecondPin = 20 }
            };

            _logger = logger;
        }

        public List<IChannel> Channels { get; }
            = new List<IChannel>();

        public Task SetPin(int pinNumber, bool on)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }
}
