using Jeepee.IO.Receiver.Application.Options;
using Jeepee.IO.Receiver.Application.Options.Models;
using Jeepee.IO.Receiver.Application.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Jeepee.IO.Receiver.Tests.Queries
{
    [TestClass]
    [TestCategory("Queries - Get Channels")]
    public class GetChannelsTests
    {
        [TestMethod]
        public async Task ReturnsChannelsDTOFromHardwareOptions()
        {
            const int ID = 0;
            const string NAME = "test channel";

            var request = new GetChannels();

            var hardwareOptions = new HardwareOptions
            {
                Channels = new List<Channel>
                {
                    new Channel { Name = NAME }
                }
            };

            var handler = new GetChannelsHandler(hardwareOptions);
            var result = await handler.Handle(request, CancellationToken.None);
            var firstChannelResult = result.First();

            Assert.AreEqual(hardwareOptions.Channels.Count, result.Count());
            Assert.AreEqual(NAME, firstChannelResult.Name);
            Assert.AreEqual(ID, firstChannelResult.Id);
        }
    }
}
