using Microsoft.AspNetCore.SignalR;
using Pippin.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jeepee.IO.Receiver.Presentation.API
{
    public class HubPinAdapter<THub> : IPinAdapter where THub : Hub
    {
        private readonly IHubContext<THub> _hubContext;

        public HubPinAdapter(IHubContext<THub> hubContext)
        {
            _hubContext = hubContext;
        }

        public void SetPin(int pinNumber, bool on)
        {
            SetPinAsync(pinNumber, on).ConfigureAwait(false).GetAwaiter().GetResult();
        }

        public async Task SetPinAsync(int pinNumber, bool on)
        {
            await _hubContext.Clients.All.SendAsync("serverupdate", pinNumber, on);
        }
    }
}
