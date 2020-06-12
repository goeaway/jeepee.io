using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Jeepee.IO.Manager.Presentation.API.Controllers
{
    public class AuthController : Controller
    {
        // below should be store this class can call to check availability of instance and add user to when available
        // private readonly JeepeeControllerStore _controllerStore;

        [HttpPost]
        [Route("login")]
        public IActionResult Authenticate()
        {
            // use google to authenticate
            return null;
        }

        [HttpPost]
        [Route("requestcontrol")]
        [Authorize]
        public IActionResult RequestControl(string jeepeeId)
        {
            // check if requested instance is available (if user already on it but expired that should be updated)
            // if not, check if this user has higher role
            // if not, return 403
            // if yes, add this user to the store as the controller of instance (time limit set inside)
            // return 200
            return null;
        }
    }
}
