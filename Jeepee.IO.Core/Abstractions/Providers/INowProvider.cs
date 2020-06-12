using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Core.Abstractions.Providers
{
    public interface INowProvider
    {
        DateTime Now { get; }
    }
}
