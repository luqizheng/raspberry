﻿using HY.IO.Ports;
using Microsoft.Extensions.Logging;
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

        public GarbageTerminalHub(GarbageTerminal garbage, ILogger<GarbageTerminalHub> logger)
        {
          

            this.Terminal = garbage;
            Logger = logger;
        }

        public ILogger<GarbageTerminalHub> Logger { get; }

        protected override void Dispose(bool disposing)
        {
         
            base.Dispose(disposing);
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            return base.OnDisconnectedAsync(exception);
        }

        public void SendMessage()
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
                    Transfer = Terminal.Transfer.PowerStatus
                }

            };
            Logger.LogDebug("send info");
            Clients.All.SendCoreAsync("status", new object[] { status });

        }
    }
}
