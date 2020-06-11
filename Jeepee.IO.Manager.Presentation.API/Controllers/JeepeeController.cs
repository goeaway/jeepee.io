using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Jeepee.IO.Manager.Presentation.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace Jeepee.IO.Manager.Presentation.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class JeepeeController : Controller
    {
        private readonly IConfiguration _configuration;

        public JeepeeController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet("getavailable")]
        public async Task<IEnumerable<string>> GetAvailableInstances()
        {
            var instanceIps = _configuration.GetSection("Jeepees").AsEnumerable().Where(c => c.Value != null).Select(c => c.Value);
            var successfulPings = new List<string>();

            if(instanceIps.Any())
            {
                var client = new HttpClient();

                foreach(var ip in instanceIps)
                {
                    var result = await client.GetAsync($"http://{ip}/ping");

                    if(result.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        successfulPings.Add(ip);
                    }
                }
            }

            return successfulPings;
        }

        [HttpPost("forwardcontrol")]
        //[Authorize]
        public async Task ForwardControl(ForwardControlModel model)
        {
            // get ip address from store
            // if none for user, this user isn't allowed to control, return 403

            var ip = "localhost:61395";

            var client = new HttpClient();

            var result = await client.PostAsync($"http://{ip}/channel/set", new StringContent(JsonConvert.SerializeObject(model), Encoding.UTF8, "application/json"));
        }
    }
}
