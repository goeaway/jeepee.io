using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Jeepee.IO.Receiver.Presentation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MonitorController : Controller
    {
        private readonly IConfiguration _configuration;
        
        public MonitorController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var mode = _configuration["Mode"];
            ViewBag.Mode = mode.ToLower();
            return View();
        }
    }
}
