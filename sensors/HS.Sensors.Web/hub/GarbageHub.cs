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
        private readonly GarbageTerminal garbage;
        Timer timer;

        public IPowerController PowerController { get; }

        public GarbageTerminalHub(GarbageTerminal garbage, IPowerController powerController)
        {
            timer = new Timer(getStatus, null, 1000, 5000);
            this.garbage = garbage;
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
                    ExhaustMain = garbage.ExhaustMain.PowerStatus,
                    ExhaustSlave = garbage.ExhaustSlave.PowerStatus,
                    GrayFan = garbage.GrayFan.PowerStatus,
                    PlasmaGenerator = garbage.PlasmaGenerator.PowerStatus,
                    Pulverizer = garbage.Pulverizer.PowerStatus,
                    Pump = garbage.Pump.PowerStatus,

                }

            };
            await Clients.All.SendCoreAsync("status", new[] { status });

        }
    }
}
