using Jeepee.IO.Controller.Application.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Jeepee.IO.Controller.Application
{
    public class Client : IDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly List<Command> _previousCommands;

        public bool Connected { get; private set; }

        public Client()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://192.168.1.87/api/")
            };

            _previousCommands = new List<Command>();

            var pingServer = Task.Run(async () => await TryConnect());

            pingServer.GetAwaiter().GetResult();
        }

        /// <summary>
        /// Ignores the request if a similar command has already been made recently
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        private bool IgnoreRequest(Command command)
        {
            // if a previous command is exactly the same as this
            return _previousCommands.Any(c =>
                c.Channel == command.Channel &&
                c.Direction == command.Direction &&
                c.On == command.On);
        }

        private void LogCommand(Command command)
        {
            // remove any commands that are of this command's channel but has the opposite direction OR On to this
            // i.e. the state of this channel has now changed due to this command, and so next time round we 
            // want to ignore the previous commands that we're about to delete!
            _previousCommands.RemoveAll(c =>
                c.Channel == command.Channel && (c.Direction != command.Direction || c.On != command.On));

            _previousCommands.Add(command);
        }

        public async Task TryConnect()
        {
            try
            {
                var response = await _httpClient.GetAsync("ping/send");
                Connected = response.StatusCode == HttpStatusCode.OK;
            }
            catch (WebException)
            {
            }
        }

        public async Task SendAsync(Command command)
        {
            if (!Connected)
                throw new ControllerNotConnectedException();

            if (IgnoreRequest(command))
                return;

            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            var posting = _httpClient.PostAsync("channel/set", content);

            LogCommand(command);

            await posting;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}
