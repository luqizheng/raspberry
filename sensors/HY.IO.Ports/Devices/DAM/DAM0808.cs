using Microsoft.Extensions.Logging;

namespace HY.IO.Ports.Devices.DAM
{
    public class DAM0808 : DAM
    {
        public DAM0808(ILogger<DAM0808> logger, string comPath) : base(logger, comPath)
        {
        }

        protected override int RelayPortsCount => 8;

        protected override int OptocouplerPorts => 8;
    }
}
