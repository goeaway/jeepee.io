using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Jeepee.IO.Core.Extensions;
using Jeepee.IO.Core.Models.DTOs;
using Jeepee.IO.Core.Models.Models;
using Jeepee.IO.Manager.Application.Abstractions;
using Jeepee.IO.Manager.Application.Commands;
using Jeepee.IO.Manager.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Jeepee.IO.Manager.Presentation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JeepeeController : Controller
    {
        private readonly IHttpContextAccessor _contextAccessor;
        private readonly IMediator _mediator;

        public JeepeeController(IMediator mediator, IHttpContextAccessor contextAccessor)
        {
            _mediator = mediator;
            _contextAccessor = contextAccessor;
        }

        [HttpGet("instancereport")]
        public Task<IEnumerable<JeepeeInstanceReportDTO>> InstanceReport()
        {
            return _mediator.Send(new GetInstanceReportRequest());
        }

        [HttpPost("forwardcontrol")]
        [Authorize]
        public async Task ForwardControl(JeepeeControlDTO dto)
        {
            var userIdent = _contextAccessor.GetUserIdent();
            await _mediator.Send(new ForwardControlRequest(userIdent, dto));
        }
    }
}
