using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jeepee.IO.Manager.Presentation.API.Models
{
    public class ForwardControlModel
    {
        public int Channel { get; set; }
        public bool Direction { get; set; }
        public bool On { get; set; }
    }
}
