using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;

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
#if ARM
        SerialDevice device;
#else
        SerialPort device;
#endif
        public SerialPortDevice(string comPath, BitRate rate)
        {
            this.comPath = comPath;
            this.rate = rate;


#if ARM
            //arm
            BaudRate rae = (BaudRate)Convert.ToInt32(rate);
            device = new SerialDevice(comPath, rae);
           device.DataReceived += Device_DataReceived;
#else
            device = new SerialPort(comPath, Convert.ToInt32(rate));
            device.ReadBufferSize = 16;
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
            if (DataReceived != null)
            {

                var bytes = new Byte[device.BytesToRead];
                device.Read(bytes, 0, device.BytesToRead);
                DataReceived(this, bytes);

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

        internal void Write(byte[] command)
        {
#if ARM
            device.Write(command);
#else
            device.Write(command, 0, command.Length);
#endif
        }
    }
}
