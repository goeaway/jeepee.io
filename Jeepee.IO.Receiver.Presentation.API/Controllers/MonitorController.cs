using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Jeepee.IO.Receiver.Presentation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonitorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
