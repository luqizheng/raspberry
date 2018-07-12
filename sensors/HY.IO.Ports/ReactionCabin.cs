using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;

namespace HY.IO.Ports
{
    public class ReactionCabin
    {
        private readonly IOptionsMonitor<DeviceSetting> setting;
        private readonly IOpenCloseController controller;

        public ReactionCabin(IOptionsMonitor<DeviceSetting> setting, IOpenCloseController controller)
        {
            this.setting = setting;
            this.controller = controller;
        }

        /// <summary>
        /// 高位
        /// </summary>
        public bool IsFull
        {
            get
            {
                return controller.IsOpen(setting.CurrentValue.OpenClosePortSetting.FullSensor);
            }
        }

        /// <summary>
        /// 低位
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                return !controller.IsOpen(setting.CurrentValue.OpenClosePortSetting.EmptySensor);
            }
        }

        /// <summary>
        /// 温度
        /// </summary>
        public decimal Temperture { get; set; }
    }
}