using Jeepee.IO.Receiver.Application.Abstractions;
using Jeepee.IO.Receiver.Application.Exceptions;
using MMALSharp;
using MMALSharp.Handlers;
using MMALSharp.Native;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Abstractions;
using Unosquare.WiringPi;

namespace Jeepee.IO.Receiver.Application.Systems
{
    public class Linux : ISystem
    {
        private readonly ILogger _logger;
        private readonly MMALCamera _camera;

        public Linux(ILogger logger)
        {
            _logger = logger;
            _camera = MMALCamera.Instance;
        }

        public List<IChannel> Channels { get; }

        public Linux()
        {
            Pi.Init<BootstrapWiringPi>();
            Channels = new List<IChannel>
            {
                new Channel { EnablePin = 18, FirstPin = 24, SecondPin = 23 },
                new Channel { EnablePin = 16, FirstPin = 21, SecondPin = 20 }
            };
        }

        public Stream GetImage()
        {
            using (var handler = new ImageStreamCaptureHandler("/home/pi/images/", "jpg"))
            {
                _logger.Information("Taking picture");
                Task.Run(async () => await _camera.TakePicture(handler, MMALEncoding.JPEG, MMALEncoding.I420));
            }

            return new MemoryStream();
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
            _camera.Cleanup();
        }
    }
}
