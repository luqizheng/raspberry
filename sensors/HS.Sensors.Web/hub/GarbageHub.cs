using HY.IO.Ports;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace HS.Sensors.Web.hub
{
    public class GarbageTerminalHub : Microsoft.AspNetCore.SignalR.Hub
    {
        private readonly GarbageTerminal Terminal;
        Timer timer;

        public IPowerController PowerController { get; }

        public GarbageTerminalHub(GarbageTerminal garbage, IPowerController powerController)
        {
            timer = new Timer(getStatus, null, 1000, 5000);
            this.Terminal = garbage;
            PowerController = powerController;
        }
        private void getStatus(object state)
        {
            PowerController.RefreshStatus();
            SendMessage().Wait();
        }


        public async Task SendMessage()
        {
            var status = new
            {
                Power = new
                {
                    ExhaustMain = Terminal.ExhaustMain.PowerStatus,
                    ExhaustSlave = Terminal.ExhaustSlave.PowerStatus,
                    GrayFan = Terminal.GrayFan.PowerStatus,
                    PlasmaGenerator = Terminal.PlasmaGenerator.PowerStatus,
                    Pulverizer = Terminal.Pulverizer.PowerStatus,
                    Pump = Terminal.Pump.PowerStatus,
                    Transfer=Terminal.Transfer.PowerStatus
                }

            };
            await Clients.All.SendCoreAsync("status", new[] { status });

        }
    }
}
