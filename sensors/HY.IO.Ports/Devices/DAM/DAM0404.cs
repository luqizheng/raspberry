using Microsoft.Extensions.Logging;

namespace HY.IO.Ports.Devices.DAM
{
    public class DAM0404 : DAM
    {
        public DAM0404(ILogger<DAM0404> logger, string comPath) : base(logger, comPath)
        {
        }

        protected override int RelayPortsCount => 4;

        protected override int OptocouplerPortsCount => 4;

        protected override int AnalogInputCount => 0;
    }
}