using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace Jeepee.IO.Controller.WinForms
{
    public class Controller
    {
        private readonly HttpClient _httpClient;
        private readonly List<Command> _commands;

        public Controller()
        {
            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://192.168.0.26")
            };

            _commands = new List<Command>
            {

            };
        }

        private bool IgnoreCommand(Command command) => 
            _commands.Any(c => 
                c.Channel == command.Channel && 
                c.Direction == command.Direction && 
                c.On == command.On);

        private void AddCommand(Command command)
        {
            _commands.RemoveAll(c => c.Channel == command.Channel && (c.Direction != command.Direction || c.On != command.On));

            _commands.Add(command);
        }

        public void Send(Command command)
        {
            if(IgnoreCommand(command))
            {
                return; 
            }

            var content = new StringContent(JsonConvert.SerializeObject(command), Encoding.UTF8, "application/json");
            _httpClient.PostAsync("channel/set", content);

            AddCommand(command);
        }
    }
}
