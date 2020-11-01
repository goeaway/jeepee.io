using Jeepee.IO.Core.Models.DTOs;
using Jeepee.IO.Receiver.Application.Commands;
using Jeepee.IO.Receiver.Application.Options;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Jeepee.IO.Receiver.Presentation.API.Hubs
{
    public class JeepeeHub : Hub
    {
        private readonly IMediator _mediator;
        private readonly HardwareOptions _options;

        public JeepeeHub(IMediator mediator, HardwareOptions options)
        {
            _mediator = mediator;
            _options = options;
        }

        public Task JoinJeepee()
        {
            return Groups.AddToGroupAsync(Context.ConnectionId, _options.Id);
        }

        public Task LeaveJeepee()
        {
            return Groups.RemoveFromGroupAsync(Context.ConnectionId, _options.Id);
        }

        public async Task Set(JeepeeControlDTO dto)
        {
            await _mediator.Send(new UpdateChannel(dto.Channel, dto.Direction, dto.On));
            await Clients.Group(_options.Id).SendAsync("Set", dto);
        }
    }
}
