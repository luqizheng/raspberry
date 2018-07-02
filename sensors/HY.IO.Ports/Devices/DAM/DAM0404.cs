using System.Text;

namespace HY.IO.Ports.Devices.DAM
{
    public class DAM0404 : DAM
    {
        public DAM0404(string comPath) : base(comPath)
        {
        }

        protected override int RelayPortsCount => 4;

        protected override int OptocouplerPorts => 4;
    }

}
