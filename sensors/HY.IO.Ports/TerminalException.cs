using System;

namespace HY.IO.Ports
{
    public class TerminalException : Exception
    {
        public TerminalException(string message) : base(message)
        {
        }
    }
}