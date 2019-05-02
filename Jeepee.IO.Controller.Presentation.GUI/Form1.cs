using Jeepee.IO.Controller.Application;
using SharpDX.XInput;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jeepee.IO.Controller.Presentation.GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            _client = new Client();

            KeyDown += async (_, e) => await KeyDownHandler(_, e);
            KeyUp += async (_, e) => await KeyUpHandler(_, e);
            FormClosing += (_, e) => Quit();

            var controller = new SharpDX.XInput.Controller(UserIndex.One);
            _cancelSource = new CancellationTokenSource();
            ControllerHandler(controller, _cancelSource.Token);
        }

        private readonly Client _client;
        private readonly CancellationTokenSource _cancelSource;

        private void Quit()
        {
            _cancelSource.Cancel();
            _client.Dispose();
        }

        private async Task KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Q)
            {
                System.Windows.Forms.Application.Exit();
            }

            switch (e.KeyCode)
            {
                case Keys.T:
                    await _client.SendAsync(new Command { Channel = 1, Direction = true, On = true });
                    break;
                case Keys.U:
                    await _client.SendAsync(new Command { Channel = 0, Direction = true, On = true });
                    break;
                case Keys.G:
                    await _client.SendAsync(new Command { Channel = 1, Direction = false, On = true });
                    break;
                case Keys.J:
                    await _client.SendAsync(new Command { Channel = 0, Direction = false, On = true });
                    break;
            }
        }

        private async Task KeyUpHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.T:
                    await _client.SendAsync(new Command { Channel = 1, Direction = true, On = false });
                    break;
                case Keys.U:
                    await _client.SendAsync(new Command { Channel = 0, Direction = true, On = false });
                    break;
                case Keys.G:
                    await _client.SendAsync(new Command { Channel = 1, Direction = false, On = false });
                    break;
                case Keys.J:
                    await _client.SendAsync(new Command { Channel = 0, Direction = false, On = false });
                    break;
            }
        }


        private Task ControllerHandler(SharpDX.XInput.Controller controller, CancellationToken token)
        {
            return Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        var gamepad = controller.GetState().Gamepad;

                        var lY = (int)gamepad.LeftThumbY;
                        var rY = (int)gamepad.RightThumbY;

                        const int OFF_THRESHOLD = 5000;

                        var l = _client.SendAsync(new Command { Channel = 1, Direction = lY > 0, On = Math.Abs(lY) > OFF_THRESHOLD });
                        var y = _client.SendAsync(new Command { Channel = 0, Direction = rY > 0, On = Math.Abs(rY) > OFF_THRESHOLD });

                        await Task.Delay(10);

                    }
                    catch (Exception)
                    {

                    }
                }
            });
        }
    }
}
