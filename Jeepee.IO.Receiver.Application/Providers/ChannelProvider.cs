using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Jeepee.IO.Receiver.Application.Abstractions;
using Jeepee.IO.Receiver.Application.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Serilog;

namespace Jeepee.IO.Receiver.Application.Providers
{
    public class ChannelProvider : IChannelProvider
    {
        private readonly ILogger _logger;
        private IEnumerable<IChannel> _cache;

        public ChannelProvider(ILogger logger)
        {
            _logger = logger;
        }

        private string GetPath()
        {
            if (Utils.IsWindows())
            {
                return "";
            }

            return "/" + Path.Combine("jeepee", "app", "channels.json");
        }

        public IEnumerable<IChannel> GetChannels()
        {
            if (_cache != null)
            {
                return _cache;
            }

            try
            {
                var path = GetPath();

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
                    _cache = JsonConvert.DeserializeObject<ChannelsConfig>(text, settings).Channels;
                    return _cache;
                }
            }
            catch (FileNotFoundException e)
            {
                _logger.Fatal(e, "Could not find channel configuration file");
                throw;
            }
            catch (Exception e)
            {
                _logger.Fatal(e, "Could not read channel configuration file");
                throw;
            }
        }
    }
}
