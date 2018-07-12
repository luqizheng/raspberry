using HY.IO.Ports.Extentions;

namespace HY.IO.Ports
{
    public class ReactionCabin
    {
        private readonly DeviceSetting setting;
        private readonly IOpenCloseController controller;

        public ReactionCabin(DeviceSetting setting, IOpenCloseController controller)
        {
            this.setting = setting;
            this.controller = controller;
        }

        /// <summary>
        /// 高位
        /// </summary>
        public bool HighLevel
        {
            get
            {
                return controller.IsOpen(setting.OpenClosePortSetting.HighLevelSensor);
            }
        }

        /// <summary>
        /// 低位
        /// </summary>
        public bool LowerLevel
        {
            get
            {
                return controller.IsOpen(setting.OpenClosePortSetting.LowLevelSensor);
            }
        }

        /// <summary>
        /// 温度
        /// </summary>
        public decimal Temperture { get; set; }
    }
}