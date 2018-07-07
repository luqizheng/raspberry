using CRC;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;

namespace HY.IO.Ports.Devices.DAM
{

    public abstract class DAM : IDisposable
    {
        private readonly byte address;
        protected static Crc crc;
        private DateTime refreshRelayTie = DateTime.Now;
        protected SerialPortDevice device;
        private object serialLockItem = 1;
        // private int expectLength = 0;
        //  private AutoResetEvent atutResetEvet = new AutoResetEvent(false);
        public IDictionary<int, bool> RelayPort { get; } = new Dictionary<int, bool>();
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
            // device.DataReceived += Device_DataReceived;

            for (int i = 0; i < this.RelayPortsCount; i++)
            {
                RelayPort[i] = false;
            }

            device.Open();
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
            lock (serialLockItem)
            {
                if (device.IsOpened)
                {
                    openByAnother = true;
                }
                else
                {
                    device.Open();
                }
                //atutResetEvet.Reset();
                Log(command, "开启");
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
            lock (serialLockItem)
            {

                if (device.IsOpened)
                {
                    openByAnother = true;

                }
                else
                {
                    device.Open();
                }

                //   atutResetEvet.Reset();
                Log(command, "关闭");
                device.Write(command);


                if (!openByAnother)
                    device.Close();
                return true;
            }
        }

        public ILogger Logger { get; }
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
            if (bytes.Length <= 1)
                return true;
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
            lock (serialLockItem)
            {

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
                Log(command, "刷新状态");
                device.Write(command);
                var f = DateTime.Now;

                Thread.Sleep(300);
                var less = DateTime.Now - f;

                var arg2 = device.Read();
                if (!Verify(arg2)) return;
                var data = arg2[3];
                for (int i = 0; i < this.RelayPortsCount; i++)
                {
                    var bit = 1 << i;
                    var result = (data & bit);
                    this.RelayPort[i] = result != 0;
                }
                Log(command, "刷新状态-end");

                if (!openByAnother)
                    device.Close();
            }


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

        public void Dispose()
        {
            this.device.Close();
        }

        /*  private void Device_DataReceived(object arg1, byte[] arg2)
          {
              try
              {
                  if (arg2.Length == 1)
                      return;
                  Log(arg2, "接收：");

                  switch (arg2[1])
                  {
                      case 0x01: //查询
                          if (!Verify(arg2)) return;
                          var data = arg2[3];
                          for (int i = 0; i < this.RelayPortsCount; i++)
                          {
                              var bit = 1 << i;
                              var result = (data & bit);
                              this.RelayPort[i] = result != 0;
                          }

                          break;
                  }
              }
              finally
              {
                  atutResetEvet.Set();

              } }*/
    }

}
