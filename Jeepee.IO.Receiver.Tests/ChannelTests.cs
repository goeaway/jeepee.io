using System;
using Jeepee.IO.Receiver.Application.Abstractions;
using Jeepee.IO.Receiver.Application.Systems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Jeepee.IO.Receiver.Tests
{
    [TestClass]
    public class ChannelTests
    {
        [TestMethod]
        public void GenerateJSON()
        {
            var channels = new List<IChannel>
            {
                new Channel { EnablePin = 18, FirstPin = 24, SecondPin = 23 },
                new Channel { EnablePin = 16, FirstPin = 21, SecondPin = 20 }
            };
            var settings = new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            Console.WriteLine(JsonConvert.SerializeObject(channels, settings));
        }
    }
}
