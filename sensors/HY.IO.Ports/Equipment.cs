using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;

namespace HY.IO.Ports
{
    public abstract class Equipment
    {
        private readonly IPowerController controller;
        private readonly IOptionsMonitor<DeviceSetting> setting;

        public Equipment(IPowerController controller, IOptionsMonitor<DeviceSetting> setting)
        {
            this.controller = controller;
            this.setting = setting;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        protected abstract int PortIndex(DeviceSetting setting);


        public Power PowerStatus
        {
            get
            {
                var index = PortIndex(setting.CurrentValue);
                return controller[index];
            }
        }
        public void TurnOn()
        {
            var index = PortIndex(setting.CurrentValue);
            controller.Turn(index, Power.On);
        }

        public void TurnOff()
        {
            var index = PortIndex(setting.CurrentValue);
            controller.Turn(index, Power.Off);
        }
    }
}
