namespace HY.IO.Ports.Extentions
{
    public class DeviceSetting
    {
        public DeviceSetting()
        {
            GrayFanRuntime = new GrayFanRuntime();
        }

        /// <summary>
        ///
        /// </summary>
        public PowerControllerSetting PowerControllerSetting { get; set; } = new PowerControllerSetting();

        /// <summary>
        ///光耦 设置
        /// </summary>
        public OpenClosePortSetting OpenClosePortSetting { get; set; } = new OpenClosePortSetting();

        /// <summary>
        ///
        /// </summary>
        public GrayFanRuntime GrayFanRuntime { get; set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class GrayFanRuntime
    {   /// <summary>
        /// 吹灰风扇休息时间
        /// </summary>
        public int GrayFanSleepSeconds { get; set; } = 30;

        /// <summary>
        /// 吹灰风扇运行时间
        /// </summary>
        public int GrayFanRunSeconds { get; set; } = 60;
    }

    /// <summary>
    ///
    /// </summary>
    public class OpenClosePortSetting
    {
        /// <summary>
        /// 高位红外探头
        /// </summary>
        public int HighLevelSensor { get; set; }

        /// <summary>
        /// 低位红外探头
        /// </summary>
        public int LowLevelSensor { get; set; }
    }

    public class PowerControllerSetting
    {
        public string ComPath { get; set; }
        public int Pulverizer { get; set; }
        public int GrayFan { get; set; }
        public int Pump { get; set; }
        public int Transfer { get; set; }
        public int ExhaustMain { get; set; }
        public int ExhaustSlave { get; set; }
        public int PlasmaGenerator { get; set; }
    }
}