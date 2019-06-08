using Jeepee.IO.Receiver.Application.Abstractions;
using Jeepee.IO.Receiver.Application.Exceptions;
using MMALSharp;
using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly MMALCamera _camera;

        public List<IChannel> Channels { get; }

        public Linux(ILogger logger)
        {
            _logger = logger;
            _camera = MMALCamera.Instance;

            Pi.Init<BootstrapWiringPi>();
            Channels = new List<IChannel>
            {
                new Channel { EnablePin = 18, FirstPin = 24, SecondPin = 23 },
                new Channel { EnablePin = 16, FirstPin = 21, SecondPin = 20 }
            };

            StreamTask();
        }

        private Task StreamTask()
        {
            return Task.Run(async() =>
            {
                try
                {
                    using (var ffCaptureHandler = FFmpegCaptureHandler.RTMPStreamer("stream", "rtmp://127.0.0.1:1935/live"))
                    using (var vidEncoder = new MMALVideoEncoder(ffCaptureHandler))
                    using (var renderer = new MMALVideoRenderer())
                    {
                        _camera.ConfigureCameraSettings();
                        var portConf = new MMALPortConfig(MMALEncoding.H264, MMALEncoding.I420, 25, 10, MMALVideoEncoder.MaxBitrateLevel4, null);

                        vidEncoder.ConfigureOutputPort(portConf);

                        _camera.Camera.VideoPort.ConnectTo(vidEncoder);
                        _camera.Camera.PreviewPort.ConnectTo(renderer);

                        await Task.Delay(2000);

                        var cts = new CancellationTokenSource(TimeSpan.FromMinutes(3));

                        await _camera.ProcessAsync(_camera.Camera.VideoPort, cts.Token);
                    }
                }
                catch (Exception e)
                {
                    _logger.Error(e, "Error ocurred when trying to stream camera feed to rtmp");
                    throw new RTMPStreamException("Streaming exception", e);
                }
            });
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
