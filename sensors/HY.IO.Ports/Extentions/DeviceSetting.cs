namespace HY.IO.Ports.Extentions
{
    public class DeviceSetting
    {
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
        public GrayFanRuntime GrayFanRuntime { get; set; } = new GrayFanRuntime();

        /// <summary>
        /// 上料模块的运行时间设置
        /// </summary>
        public TransferRuntime TransferRuntime { get; set; } = new TransferRuntime();
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

    public class TransferRuntime
    {
        /// <summary>
        /// 闸刀启动时间
        /// </summary>
        public int PulverizerRuntimerSeconds { get; set; } = 30;

        /// <summary>
        /// 传输带持续时间
        /// </summary>
        public int TransferStopAfterPulverizerStop { get; set; } = 60;

        /// <summary>
        ///  载碎料器启动之后，传输带开始启动
        /// </summary>
        public int TransferStartAfterPulverizerStart { get; set; } = 15;
    }

    /// <summary>
    ///
    /// </summary>
    public class OpenClosePortSetting
    {
        /// <summary>
        /// 高位红外探头
        /// </summary>
        public int FullSensor { get; set; }

        /// <summary>
        /// 低位红外探头
        /// </summary>
        public int EmptySensor { get; set; }
    }

    public class PowerControllerSetting
    {
        public string ComPath { get; set; }
        public int Pulverizer { get; set; }
        public int GrayFan { get; set; }
        public int PrimaryPump { get; set; }

        public int SecondaryPump { get; set; }

        public int Transfer { get; set; }
        public int ExhaustMain { get; set; }
        public int ExhaustSlave { get; set; }
        public int PrimaryPlasmaGenerator { get; set; }
        public int SecondaryPlasmaGenerator { get; set; }
        public int UVLight { get; set; }
    }
}