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
    public static class ConnectionTracker
    {
        public static bool UserConnected => !string.IsNullOrEmpty(ConnectedUser);
        public static string ConnectedUser = string.Empty;
    }

    public class JeepeeHub : Hub
    {
        private readonly IMediator _mediator;

        public JeepeeHub(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override Task OnConnectedAsync()
        {
            // we only allow one connection at a time on this hub
            if(ConnectionTracker.UserConnected)
            {
                Context.Abort();
            }
            else
            {
                // if allowed, set the tracker to hold this connection id
                ConnectionTracker.ConnectedUser = Context.ConnectionId;
            }

            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            // untrack if the allowed connection is disconnecting, so other connections can now use
            if(Context.ConnectionId == ConnectionTracker.ConnectedUser)
            {
                ConnectionTracker.ConnectedUser = string.Empty;
            }
            return base.OnDisconnectedAsync(exception);
        }

        public async Task Set(JeepeeControlDTO dto)
        {
            // only act on the request if the allowed connection is making it
            if(Context.ConnectionId == ConnectionTracker.ConnectedUser)
            {
                await _mediator.Send(new UpdateChannel(dto.Channel, dto.Direction, dto.On));
                await Clients.All.SendAsync("Set", dto);
            }
        }
    }
}
