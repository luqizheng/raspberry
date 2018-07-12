using System;

namespace HY.IO.Ports
{
    public class OpenCloseEventArgs : EventArgs
    {
        public int PortIndex { get; set; }
        public bool IsOpen { get; set; }
    }
}