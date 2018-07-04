
using System;
using HY.IO.Ports;
using CRC;
using HY.IO.Ports.Devices.DAM;
namespace ConsoleApp1
{
    public class Program
    {
        static void Main(string[] args)
        {
            var ports = SerialPortDevice.GetPortNames();
            SerialPortDevice port = new SerialPortDevice("COM3", BitRate.B9600);
            //var port = new System.IO.Ports.SerialPort("/dev/ttyUSB0", 9600);
            /*
            FE 05 00 00 FF 00 98 35
             FE 05 00 01 FF 00 C9 F5*/

            //port.PortName = "/dev/ttyUSB0";
            //port.BaudRate = 9600;

            port.Open();
            var data = new byte[]
            {
                0xFE, 0x05 ,0x00 ,0x01 ,0xFF ,0x00 ,0xc9 ,0xf5
            };

            //FE 05 00 01 FF 00 C9 F5
            port.Write(data);

            data = new byte[]
            {
                0xFE , 0x05 , 0x00 , 0x00 , 0x00 , 0x00 , 0xD9 , 0xC5
            };
            port.Write(data);

            port.Close();




        }
    }
}
