namespace HY.IO.Ports.Devices.DAM
{
    public class DAM0808 : DAM
    {
        public DAM0808(string comPath, byte address = 254) : base(comPath, address)
        {
        }

        protected override int RelayPortsCount => 8;

        protected override int OptocouplerPorts => 8;
    }
}
