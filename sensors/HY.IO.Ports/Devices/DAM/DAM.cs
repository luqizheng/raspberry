using CRC;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;

namespace HY.IO.Ports.Devices.DAM
{
    public abstract class DAM : IDisposable
    {
        protected static Crc crc;

        protected SerialPortDevice device;

        private readonly byte address;

        private DateTime refreshRelayTie = DateTime.Now;

        static DAM()
        {
            crc = new Crc(CrcStdParams.StandartParameters[CrcAlgorithms.Crc16Modbus]);
            crc.Initialize();
        }

        public DAM(ILogger logger, string comPath, byte address = 0xfe)
        {
            if (string.IsNullOrEmpty(comPath))
            {
                throw new ArgumentException("message", nameof(comPath));
            }

            Logger = logger;
            ComPath = comPath;
            this.address = address;

            var ports = SerialPortDevice.GetPortNames();
            device = new SerialPortDevice(logger, comPath, BitRate.B9600);

            for (int i = 0; i < this.RelayPortsCount; i++)
            {
                RelayPort[i] = false;
            }

            for (int i = 0; i < this.OptocouplerPortsCount; i++)
                OptocouplerPort[i] = false;

            device.Open();
        }

        public enum QueryType
        {
            Relay = 1,
            Optocoupler = 2,
        }

        public string ComPath { get; }

        public ILogger Logger { get; }

        public IDictionary<int, bool> OptocouplerPort
        {
            get;
        } = new Dictionary<int, bool>();

        // private int expectLength = 0;
        //  private AutoResetEvent atutResetEvet = new AutoResetEvent(false);
        public IDictionary<int, bool> RelayPort { get; } = new Dictionary<int, bool>();

        protected abstract int OptocouplerPortsCount { get; }
        protected abstract int RelayPortsCount { get; }

        public virtual bool Close(int port)
        {
            var command = MakeOpenCloseCommand(port != -1, port, false);
            var openByAnother = false;

            if (device.IsOpened)
            {
                openByAnother = true;
            }
            else
            {
                device.Open();
            }

            //   atutResetEvet.Reset();
            // Log(command, "关闭");
            device.Write(command);

            if (!openByAnother)
                device.Close();
            return true;
        }

        public void Dispose()
        {
            this.device.Close();
        }

        public bool IsOpen(int port)
        {
            return RelayPort[port];
        }

        public virtual bool Open(int port)
        {
            var command = MakeOpenCloseCommand(port != -1, port, true);
            var openByAnother = false;

            if (device.IsOpened)
            {
                openByAnother = true;
            }
            else
            {
                device.Open();
            }
            //atutResetEvet.Reset();
            // Log(command, "开启");
            device.Write(command);

            if (!openByAnother)
                device.Close();

            return true;
        }

        public void RefreshOptocouplerStatus()
        {
            const int port = -1; //查所有,暂时查询所有，因为返回结果速度差不多，没必要单个查
            var command = MakeQueryCommand(port == -1 ? 0 : port, port == -1 ? this.RelayPortsCount : 1, QueryType.Optocoupler);

            var openByAnother = false;
            if (device.IsOpened)
            {
                openByAnother = true;
            }
            else
            {
                device.Open();
            }

            //                atutResetEvet.Reset();
            //Log(command, "刷新状态");
            var arg2 = device.Write(command);
            if (!Verify(arg2)) return;
            var data = arg2[3];
            for (int i = 0; i < this.RelayPortsCount; i++)
            {
                var bit = 1 << i;
                var result = (data & bit);
                this.RelayPort[i] = result != 0;
            }
            //Log(command, "刷新状态-end");

            if (!openByAnother)
                device.Close();
        }

        /// <summary>
        /// 刷新所有端口的状态
        /// </summary>
        public void RefreshRelayStatus()
        {
            const int port = -1; //查所有
            var command = MakeQueryCommand(port == -1 ? 0 : port, port == -1 ? this.RelayPortsCount : 1, QueryType.Relay);

            var openByAnother = false;
            if (device.IsOpened)
            {
                openByAnother = true;
            }
            else
            {
                device.Open();
            }

            //                atutResetEvet.Reset();
            //Log(command, "刷新状态");
            var arg2 = device.Write(command);
            if (!Verify(arg2)) return;
            var data = arg2[3];
            for (int i = 0; i < this.RelayPortsCount; i++)
            {
                var bit = 1 << i;
                var result = (data & bit);
                this.RelayPort[i] = result != 0;
            }
            //Log(command, "刷新状态-end");

            if (!openByAnother)
                device.Close();
        }

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

        protected byte[] MakeQueryCommand(int start, int length, QueryType type)
        {
            //FE 01 00 00 00 04 29 C6;
            var result = new byte[8];
            result[0] = address;
            result[1] = Convert.ToByte(type);
            result[3] = Convert.ToByte(start);
            result[5] = Convert.ToByte(length);

            var checkSum = crc.ComputeHash(result, 0, 6);
            result[6] = checkSum[checkSum.Length - 1];
            result[7] = checkSum[checkSum.Length - 2];
            return result;
        }

        protected bool Verify(byte[] bytes)
        {
            if (bytes.Length <= 1)
                return false;
            var checkSum = crc.ComputeHash(bytes, 0, bytes.Length - 2);
            return bytes[bytes.Length - 2] == checkSum[checkSum.Length - 1] && bytes[bytes.Length - 1] == checkSum[checkSum.Length - 2];
        }

        private void Log(byte[] arg2, string message)
        {
            var console = new string[arg2.Length];
            for (var i = 0; i < arg2.Length; i++)
            {
                console[i] = arg2[i].ToString("X2");
            }

            Logger.LogDebug(message + " " + string.Join(" ", console));
        }
    }
}