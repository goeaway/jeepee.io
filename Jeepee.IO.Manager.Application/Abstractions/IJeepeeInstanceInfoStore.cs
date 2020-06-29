using Jeepee.IO.Core.Models.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Manager.Application.Abstractions
{
    public interface IJeepeeInstanceInfoStore
    {
        JeepeeInstanceInfo GetInfoForId(string instanceId);
        IEnumerable<JeepeeInstanceInfo> GetAll();
    }
}
