using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading;

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
            {
#if ARM
                return device.IsOpened;
#else
                return device.IsOpen;
#endif
            }
        }
        public Action<object, byte[]> DataReceived { get; internal set; }
        public ILogger Logger { get; }
#if ARM
        SerialDevice device;
#else
        SerialPort device;
#endif
        public SerialPortDevice(ILogger logger, string comPath, BitRate rate)
        {
            Logger = logger;
            this.comPath = comPath;
            this.rate = rate;


#if ARM
            //arm
            BaudRate rae = (BaudRate)Convert.ToInt32(rate);
            device = new SerialDevice(comPath, rae);
      
           device.DataReceived += Device_DataReceived;
#else
            device = new SerialPort(comPath, Convert.ToInt32(rate));
            device.ReadBufferSize = 128;
            device.DataReceived += Device_DataReceived;

#endif
        }


#if ARM
       private void Device_DataReceived(object sender, byte[] e)
        {
            if (DataReceived != null)
            {
              
                    DataReceived(this, e);
                
            }
        }
#else
        private void Device_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (DataReceived != null && device.IsOpen)
            {
                try
                {
                    Thread.Sleep(300);//等待300毫秒，结果。否则buffer预计会不事完整的数据

                    var len = device.BytesToRead;

                    var bytes = new Byte[len];
                    device.Read(bytes, 0, len);
                    DataReceived(this, bytes);

                }
                catch (Exception ex)
                {

                }

            }
        }
#endif
        public void Open()
        {

            device.Open();
        }

        public void Close()
        {
            device.Close();
        }

        public void Write(byte[] command)
        {
#if ARM
            device.Write(command);
#else
            device.Write(command, 0, command.Length);
#endif
        }
    }
}
