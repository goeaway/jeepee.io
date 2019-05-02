using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Controller.Application
{
    public class Command
    {
        public int Channel { get; set; }
        public bool Direction { get; set; }
        public bool On { get; set; }
    }
}
