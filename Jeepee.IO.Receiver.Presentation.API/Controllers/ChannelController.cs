using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jeepee.IO.Core.Models.DTOs;
using Jeepee.IO.Receiver.Application.Commands;
using Jeepee.IO.Receiver.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jeepee.IO.Receiver.Presentation.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class ChannelController : Controller
    {
        private readonly IMediator _mediator;

        public ChannelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("getchannels")]
        public Task<IEnumerable<ChannelDTO>> GetChannels()
        {
            return _mediator.Send(new GetChannels());
        }

        [HttpPost("set")]
        public IActionResult Set(JeepeeControlDTO dto)
        {
            _mediator.Send(new UpdateChannel(dto.Channel, dto.Direction, dto.On));
            return Ok();
        }
    }
}