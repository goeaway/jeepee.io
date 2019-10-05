using System;
using System.Collections.Generic;
using System.Text;

namespace Jeepee.IO.Receiver.Application
{
    public static class Utils
    {
        public static bool IsWindows()
        {
            return Environment.OSVersion.Platform == PlatformID.Win32NT;
        }
    }
}
