using Jeepee.IO.Core.Abstractions.Providers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Core.Providers
{
    public class NowProvider : INowProvider
    {
        public DateTime Now => DateTime.Now;
    }
}
