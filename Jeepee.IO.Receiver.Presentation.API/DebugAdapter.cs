using Microsoft.AspNetCore.SignalR;
using Pippin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace Jeepee.IO.Receiver.Presentation.API
{
    public class DebugAdapter : IPinAdapter
    {
        private readonly ILogger _logger;

        public DebugAdapter(ILogger logger)
        {
            _logger = logger;
        }

        public void SetPin(int pinNumber, bool on)
        {
            _logger.Debug($"Setting pin {pinNumber} to {(on ? "on" : "off")}");
        }

        public Task SetPinAsync(int pinNumber, bool on) => Task.Run(() => SetPin(pinNumber, on));
    }
}
