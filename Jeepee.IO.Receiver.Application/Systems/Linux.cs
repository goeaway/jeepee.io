using Jeepee.IO.Receiver.Application.Abstractions;
using Jeepee.IO.Receiver.Application.Exceptions;
using MMALSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MMALSharp.Components;
using MMALSharp.Native;
using MMALSharp.Ports;
using MMALSharp.FFmpeg;
using MMALSharp.Handlers;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Jeepee.IO.Receiver.Application.Systems
{
    public class Linux : ISystem
    {
        private readonly ILogger _logger;

        public List<IChannel> Channels { get; }

        public Linux(ILogger logger, IChannelProvider channelProvider)
        {
            _logger = logger;
            Pi.Init<BootstrapWiringPi>();
            Channels = channelProvider.GetChannels().ToList();
            //Channels = new List<IChannel>
            //{
            //    new Channel { EnablePin = 18, FirstPin = 24, SecondPin = 23 },
            //    new Channel { EnablePin = 16, FirstPin = 21, SecondPin = 20 }
            //};
        }

        public Task SetPin(int pinNum, bool on)
        {
            return Task.Run(() =>
            {
                try
                {
                    var pin = Pi.Gpio[pinNum];
                    pin.PinMode = GpioPinDriveMode.Output;
                    pin.Write(on);
                }
                catch (Exception e)
                {
                    throw new SetPinException($"failed to set pin {pinNum} to {(on ? "on" : "off")}", e);
                }
            });
        }

        public void Dispose()
        {
        }
    }
}
