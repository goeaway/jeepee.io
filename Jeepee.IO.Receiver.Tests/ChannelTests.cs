using System;
using Jeepee.IO.Receiver.Application.Abstractions;
using Jeepee.IO.Receiver.Application.Systems;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Jeepee.IO.Receiver.Application.Providers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.IO;
using Jeepee.IO.Receiver.Application.Models;

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

        private IEnumerable<IChannel> Data(string path)
        {

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"Could not find channel configuration file at '{path}'");
            }

            var settings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            };

            using (var reader = File.OpenText(path))
            {
                var text = reader.ReadToEnd();
                return JsonConvert.DeserializeObject<ChannelsConfig>(text, settings).Channels;
            }
        }

        [TestMethod]
        public void ReadFromFile()
        {
            const string path = @"S:\Code\pi\pis\tank\jeepee-docker\compose\channels.json";

            var data = Data(path);
        }
    }
}
