using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Jeepee.IO.Receiver.Application.Commands;
using Jeepee.IO.Receiver.Presentation.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Jeepee.IO.Receiver.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChannelController : Controller
    {
        private readonly IMediator _mediator;

        public ChannelController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Route("set")]
        public IActionResult Set(UpdateChannelModel model)
        {
            _mediator.Send(new UpdateChannel(model.Channel, model.Direction, model.On));
            return Ok();
        }
    }
}