namespace HY.IO.Ports.Extentions
{
    public class DeviceSetting
    {
        public PowerControllerSetting PowerControllerSetting { get; set; } = new PowerControllerSetting();

        public int GrayFanSleepSeconds = 30;
        public int GrayFanRunSeconds = 60;

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
