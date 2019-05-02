using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jeepee.IO.Receiver.Presentation.API.Models
{
    public class UpdateChannelModel
    {
        public int Channel { get; set; }
        public bool Direction { get; set; }
        public bool On { get; set; }
    }
}
