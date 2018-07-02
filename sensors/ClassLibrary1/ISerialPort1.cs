using System.IO.Ports;
namespace HY.SerialPort
{
    public interface ISerialPort1
    {
        void Send(params byte[] data);
    }

    public class SerialPortConnection
    {
        public SerialPortConnection()
        {
            var ports = SerialDevice.GetPortNames();
            SerialDevice port = new SerialDevice("/dev/ttyUSB0", BaudRate.B9600);
            //var port = new System.IO.Ports.SerialPort("/dev/ttyUSB0", 9600);
            /*
            FE 05 00 00 FF 00 98 35
             FE 05 00 01 FF 00 C9 F5*/

            //port.PortName = "/dev/ttyUSB0";
            //port.BaudRate = 9600;

            port.Open();
            var data = new byte[]
            {
                0xFE, 0x05 ,0x00 ,0x00 ,0xFF ,0x00 ,0x98 ,0x35
            };

            
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