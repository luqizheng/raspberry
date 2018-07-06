using HY.IO.Ports.Helper;
using Microsoft.Extensions.Logging;
using RJCP.IO.Ports;
using System;
using System.IO.Ports;

namespace HY.IO.Ports
{
    public enum BitRate
    {
        B1200 = 1200,
        B2400 = 2400,
        B4800 = 4800,
        B9600 = 9600,
        B19200 = 19200,
        B38400 = 38400,
        B57600 = 57600,
        B1152000 = 115200
    }
    public class SerialPortDevice
    {
        public static string GetPortNames()
        {
            return null;
        }
        private readonly string comPath;
        private readonly BitRate rate;

        public bool IsOpened
        {
            get
            {                return device.IsOpen;

            }
        }
        public Action<object, byte[]> DataReceived { get; internal set; }
        public ILogger Logger { get; }
        SerialPortStream device;

        public SerialPortDevice(ILogger logger, string comPath, BitRate rate)
        {
            Logger = logger;
            this.comPath = comPath;
            this.rate = rate;

            device = new SerialPortStream(comPath, Convert.ToInt32(rate));


        }



        public void Open()
        {
            device.Open();
        }

        public void Close()
        {
            device.Close();
        }
        public byte[] Read()
        {

            var len = device.BytesToRead;

            var r = new Byte[len];
            device.Read(r, 0, len);
            Logger.LogDebug("Reading {0}", BitHelper.BitToString(r));

            Logger.LogDebug("ARM Reading {0}", BitHelper.BitToString(r));
            return r;
        }
        public void Write(byte[] command)
        {

            Logger.LogDebug("ARM Sending {0}", BitHelper.BitToString(command));
            device.Write(command, 0, command.Length);

        }
    }
}
