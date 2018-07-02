using CRC;
using System;
using System.Collections.Generic;
using System.IO.Ports;

namespace HY.IO.Ports.Devices.DAM
{
    public abstract class DAM
    {
        private readonly byte address;
        protected static Crc crc;
        private DateTime refreshRelayTie = DateTime.Now;
        protected SerialPortDevice device;

        public IDictionary<int, bool> RelayPort { get; } = new Dictionary<int, bool>();
        static DAM()
        {
            crc = new Crc(CrcStdParams.StandartParameters[CrcAlgorithms.Crc16Modbus]);
            crc.Initialize();
        }
        private System.Threading.AutoResetEvent reset = new System.Threading.AutoResetEvent(false);
        public DAM(string comPath, byte address = 0xfe)
        {
            if (string.IsNullOrEmpty(comPath))
            {
                throw new ArgumentException("message", nameof(comPath));
            }

            ComPath = comPath;
            this.address = address;

            var ports = SerialPortDevice.GetPortNames();
            device = new SerialPortDevice(comPath, BitRate.B9600);
            device.DataReceived += Device_DataReceived;

        }
        protected abstract int RelayPortsCount { get; }
        protected abstract int OptocouplerPorts { get; }
        public bool IsOpen(int port)
        {
            return RelayPort[port];
        }

        public virtual bool Open(int port)
        {
            var command = MakeOpenCloseCommand(port != -1, port, true);
            var openByAnother = false;
            lock (this)
            {

          
                if (!device.IsOpened)
                {
                    openByAnother = true;
                    device.Open();
                }

                device.Write(command);
                if (!openByAnother)
                    device.Close();

                return true;

            }

        }
        public virtual bool Close(int port)
        {
            var command = MakeOpenCloseCommand(port != -1, port, false);
            var openByAnother = false;
            lock (this)
            {
             
                if (!device.IsOpened)
                {
                    openByAnother = true;
                    device.Open();
                }

                device.Write(command);
                if (!openByAnother)
                    device.Close();
                return true;
            }
        }
        public string ComPath { get; }
        /// <summary>
        /// 继电器开关
        /// </summary>
        /// <param name="IsSignle"></param>
        /// <param name="position"></param>
        /// <param name="openOrClose"></param>
        /// <returns></returns>
        protected byte[] MakeOpenCloseCommand(bool IsSignle, int position, bool openOrClose)
        {
            var result = new byte[8];
            result[0] = address;
            result[1] = Convert.ToByte(IsSignle ? 0x05 : 0x00);

            result[3] = Convert.ToByte(position);

            result[4] = Convert.ToByte(openOrClose ? 0xff : 0x0);

            var checkSum = crc.ComputeHash(result, 0, 6);
            result[6] = checkSum[checkSum.Length - 1];
            result[7] = checkSum[checkSum.Length - 2];
            return result;
        }
        protected byte[] MakeQueryCommand(int start, int length)
        {
            //FE 01 00 00 00 04 29 C6;
            var result = new byte[8];
            result[0] = address;
            result[1] = 0x01;
            result[3] = Convert.ToByte(start);
            result[5] = Convert.ToByte(length);

            var checkSum = crc.ComputeHash(result, 0, 6);
            result[6] = checkSum[checkSum.Length - 1];
            result[7] = checkSum[checkSum.Length - 2];
            return result;
        }
        protected bool Verify(byte[] bytes)
        {
            var checkSum = crc.ComputeHash(bytes, 0, bytes.Length - 2);
            return bytes[bytes.Length - 2] == checkSum[checkSum.Length - 1] && bytes[bytes.Length - 1] == checkSum[checkSum.Length - 2];
        }
        /// <summary>
        /// 刷新所有端口的状态
        /// </summary>
        public void RefreshRelayStatus(bool forceRefres = false)
        {
          
            const int port = -1; //查所有
            var command = MakeQueryCommand(port == -1 ? 0 : port, port == -1 ? this.RelayPortsCount : 1);
            lock (this)
            {
                var openByAnother = false;
                if (!device.IsOpened)
                {
                    openByAnother = true;
                    device.Open();
                }

                device.Write(command);
                reset.WaitOne();
                if (!openByAnother)
                    device.Close();
            }

        }

        private void Device_DataReceived(object arg1, byte[] arg2)
        {
            if (!Verify(arg2))
                return;
            switch (arg2[1])
            {
                case 0x01: //查询

                    var data = arg2[3];
                    for (int i = 0; i < this.RelayPortsCount; i++)
                    {
                        var bit = 1 << i;
                        var result = (data & bit);
                        this.RelayPort[i] = result != 0;
                    }
                    reset.Set();
                    break;
            }
        }
    }

}
