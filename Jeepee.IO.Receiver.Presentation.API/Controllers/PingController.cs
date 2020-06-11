using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Jeepee.IO.Receiver.Presentation.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PingController : Controller
    {
        public string Send()
        {
            return "Pong";
        }
    }
}