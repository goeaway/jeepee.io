using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Jeepee.IO.Controller.WinForms
{
    public class Command
    {
        public int Channel { get; set; }
        public bool Direction { get; set; }
        public bool On { get; set; }

        public override bool Equals(object obj) =>
            obj is Command command &&
                   Channel == command.Channel &&
                   Direction == command.Direction &&
                   On == command.On;

        public override int GetHashCode() => HashCode.Combine(Channel, Direction, On);

        
    }
}
