using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Logging;

namespace HY.IO.Ports.Devices.DAM
{
    public class DAM16CC : DAM
    {
        public DAM16CC(ILogger logger, string comPath, byte address = 254) : base(logger, comPath, address)
        {
        }

        protected override int OptocouplerPortsCount => 12;

        protected override int RelayPortsCount => 16;

        protected override int AnalogInputCount => 12;
    }
}