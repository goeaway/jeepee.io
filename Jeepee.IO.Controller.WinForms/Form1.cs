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
        private readonly Controller _controller;

        public Form1()
        {
            InitializeComponent();

            KeyDown += KeyDownHandler;
            KeyUp += KeyUpHandler;

            _controller = new Controller();
        }

        private void KeyUpHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                Application.Exit();
            }

            Command command = null;
            switch (e.KeyCode)
            {
                case Keys.NumPad7:
                    command = new Command { Channel = 1, Direction = true, On = false };
                    break;
                case Keys.NumPad9:
                    command = new Command { Channel = 0, Direction = true, On = false };
                    break;
                case Keys.NumPad1:
                    command = new Command { Channel = 1, Direction = false, On = false };
                    break;
                case Keys.NumPad3:
                    command = new Command { Channel = 0, Direction = false, On = false };
                    break;
            }

            if(command != null)
            {
                _controller.Send(command);   
            }
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                Application.Exit();
            }

            Command command = null;
            switch (e.KeyCode)
            {
                case Keys.NumPad7:
                    command = new Command { Channel = 1, Direction = true, On = true };
                    break;
                case Keys.NumPad9:
                    command = new Command { Channel = 0, Direction = true, On = true };
                    break;
                case Keys.NumPad1:
                    command = new Command { Channel = 1, Direction = false, On = true };
                    break;
                case Keys.NumPad3:
                    command = new Command { Channel = 0, Direction = false, On = true };
                    break;
            }

            if (command != null)
            {
                _controller.Send(command);
            }
        } 

    }
}
