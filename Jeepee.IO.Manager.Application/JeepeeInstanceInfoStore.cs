using Jeepee.IO.Core.Models.Models;
using Jeepee.IO.Manager.Application.Abstractions;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace Jeepee.IO.Manager.Application
{
    public class JeepeeInstanceInfoStore : IJeepeeInstanceInfoStore
    {
        private IEnumerable<JeepeeInstanceInfo> _info;

        public JeepeeInstanceInfoStore(IConfiguration configuration)
        {
            _info = configuration
                .GetSection("Jeepees")
                .AsEnumerable()
                .Where(c => c.Value != null)
                .Select(c => JsonConvert.DeserializeObject<JeepeeInstanceInfo>(c.Value));
        }

        public JeepeeInstanceInfo GetInfoForId(string instanceId)
        {
            return _info.FirstOrDefault(i => i.Id == instanceId);
        }
        
        public IEnumerable<JeepeeInstanceInfo> GetAll()
        {
            return _info.ToImmutableArray();
        }
    }
}
