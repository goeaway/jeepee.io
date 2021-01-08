using Jeepee.IO.Receiver.Application.Commands;
using Jeepee.IO.Receiver.Application.Options;
using Jeepee.IO.Receiver.Application.Options.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Pippin.Core;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeepee.IO.Receiver.Tests.Commands
{
    [TestClass]
    [TestCategory("Commands - Update Channel")]
    public class UpdateChannelTests
    {
        [TestMethod]
        public async Task LogsInformationOnCall()
        {
            const int CHANNEL = 0;
            const bool DIRECTION = true;
            const bool ON = true;

            var request = new UpdateChannel(CHANNEL, DIRECTION, ON);

            var loggerMock = new Mock<ILogger>();
            var pinSetterMock = new Mock<IPinSetter>();

            var hardwareOptions = new HardwareOptions
            {
                Channels = new List<Channel>
                {
                    new Channel()
                }
            };

            var handler = new UpdateChannelHandler(loggerMock.Object, pinSetterMock.Object, hardwareOptions);
            await handler.Handle(request, CancellationToken.None);

            loggerMock.Verify(mock => 
                mock.Information(It.IsAny<string>(), CHANNEL, DIRECTION, ON),
                Times.Once
            );
        }

        [TestMethod]
        public async Task UsesPinSetterToSetEnablePinToOn()
        {
            const int CHANNEL = 0;
            const bool DIRECTION = true;
            const bool ON = true;

            const int ENABLE = 20;
            const int ONE = 30;
            const int TWO = 40;

            var request = new UpdateChannel(CHANNEL, DIRECTION, ON);

            var loggerMock = new Mock<ILogger>();
            var pinSetterMock = new Mock<IPinSetter>();

            var hardwareOptions = new HardwareOptions
            {
                Channels = new List<Channel>
                {
                    new Channel { Enable = ENABLE, One = ONE, Two = TWO }
                }
            };

            var handler = new UpdateChannelHandler(loggerMock.Object, pinSetterMock.Object, hardwareOptions);
            await handler.Handle(request, CancellationToken.None);

            pinSetterMock.Verify(mock =>
                mock.SetPinAsync(ENABLE, ON),
                Times.Once
            );
        }

        [TestMethod]
        public async Task UsesPinSetterToSetOnePinToDirection()
        {
            const int CHANNEL = 0;
            const bool DIRECTION = true;
            const bool ON = true;

            const int ENABLE = 20;
            const int ONE = 30;
            const int TWO = 40;

            var request = new UpdateChannel(CHANNEL, DIRECTION, ON);

            var loggerMock = new Mock<ILogger>();
            var pinSetterMock = new Mock<IPinSetter>();

            var hardwareOptions = new HardwareOptions
            {
                Channels = new List<Channel>
                {
                    new Channel { Enable = ENABLE, One = ONE, Two = TWO }
                }
            };

            var handler = new UpdateChannelHandler(loggerMock.Object, pinSetterMock.Object, hardwareOptions);
            await handler.Handle(request, CancellationToken.None);

            pinSetterMock.Verify(mock =>
                mock.SetPinAsync(ONE, DIRECTION),
                Times.Once
            );
        }

        [TestMethod]
        public async Task UsesPinSetterToSetTwoPinToAntiDirection()
        {
            const int CHANNEL = 0;
            const bool DIRECTION = true;
            const bool ON = true;

            const int ENABLE = 20;
            const int ONE = 30;
            const int TWO = 40;

            var request = new UpdateChannel(CHANNEL, DIRECTION, ON);

            var loggerMock = new Mock<ILogger>();
            var pinSetterMock = new Mock<IPinSetter>();

            var hardwareOptions = new HardwareOptions
            {
                Channels = new List<Channel>
                {
                    new Channel { Enable = ENABLE, One = ONE, Two = TWO }
                }
            };

            var handler = new UpdateChannelHandler(loggerMock.Object, pinSetterMock.Object, hardwareOptions);
            await handler.Handle(request, CancellationToken.None);

            pinSetterMock.Verify(mock =>
                mock.SetPinAsync(TWO, !DIRECTION),
                Times.Once
            );
        }
    }
}
