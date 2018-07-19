using CRC;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HY.IO.Ports.Devices.Delta
{
    public class Vfd
    {
        protected static Crc crc;

        static Vfd()
        {
            crc = new Crc(CrcStdParams.StandartParameters[CrcAlgorithms.Crc16Modbus]);
            crc.Initialize();
        }

        public Vfd(ILogger logger, string comPath, byte address = 0xfe)
        {
            Logger = logger;
        }

        public ILogger Logger { get; }
    }
}