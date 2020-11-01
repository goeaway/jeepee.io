using Jeepee.IO.Receiver.Application.Options.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Receiver.Application.Options
{
    public class HardwareOptions
    {
        public const string Key = "Hardware";

        public IList<Channel> Channels { get; set; }
            = new List<Channel>();
        
        public string Id { get; set; }
    }
}
