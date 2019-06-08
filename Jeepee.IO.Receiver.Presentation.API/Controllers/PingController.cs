using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Jeepee.IO.Receiver.Presentation.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PingController : Controller
    {
        [HttpGet]
        [Route("send")]
        public IActionResult Send()
        {
            return Content("Pong");
        }
    }
}