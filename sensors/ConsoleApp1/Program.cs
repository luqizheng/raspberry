using HY.Sensors.Tempture;
using HY.SerialPort;
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
            var comPath = "COM3";
            var dam404 = new DAM0404(comPath);

            var publizer = new DAMPowerController(dam404);
    
    


        }
    }
}
