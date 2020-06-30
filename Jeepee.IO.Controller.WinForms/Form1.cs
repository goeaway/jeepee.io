using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jeepee.IO.Controller.WinForms
{
    public partial class Form1 : Form
    {
        private readonly HttpClient _httpClient;

        public Form1()
        {
            InitializeComponent();

            KeyDown += KeyDownHandler;
            KeyUp += KeyUpHandler;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri("http://192.168.0.26")
            };
        }

        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                Application.Exit();
            }

            StringContent content = null;
            switch (e.KeyCode)
            {
                case Keys.NumPad7:
                    content = new StringContent(JsonConvert.SerializeObject(new { Channel = 1, Direction = true, On = true }), Encoding.UTF8, "application/json");
                    break;
                case Keys.NumPad9:
                    content = new StringContent(JsonConvert.SerializeObject(new { Channel = 0, Direction = true, On = true }), Encoding.UTF8, "application/json");
                    break;
                case Keys.NumPad1:
                    content = new StringContent(JsonConvert.SerializeObject(new { Channel = 1, Direction = false, On = true }), Encoding.UTF8, "application/json");
                    break;
                case Keys.NumPad3:
                    content = new StringContent(JsonConvert.SerializeObject(new { Channel = 0, Direction = false, On = true }), Encoding.UTF8, "application/json");
                    break;
            }

            if(content != null)
            {
                _httpClient.PostAsync("channel/set", content);
            }
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                Application.Exit();
            }

            StringContent content = null;
            switch (e.KeyCode)
            {
                case Keys.NumPad7:
                    content = new StringContent(JsonConvert.SerializeObject(new { Channel = 1, Direction = true, On = false }), Encoding.UTF8, "application/json");
                    break;
                case Keys.NumPad9:
                    content = new StringContent(JsonConvert.SerializeObject(new { Channel = 0, Direction = true, On = false }), Encoding.UTF8, "application/json");
                    break;
                case Keys.NumPad1:
                    content = new StringContent(JsonConvert.SerializeObject(new { Channel = 1, Direction = false, On = false }), Encoding.UTF8, "application/json");
                    break;
                case Keys.NumPad3:
                    content = new StringContent(JsonConvert.SerializeObject(new { Channel = 0, Direction = false, On = false }), Encoding.UTF8, "application/json");
                    break;
            }

            if (content != null)
            {
                _httpClient.PostAsync("channel/set", content);
            }
        } 

    }
}
