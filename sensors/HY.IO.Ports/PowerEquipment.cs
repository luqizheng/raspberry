using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;
using System;

namespace HY.IO.Ports
{
    public abstract class PowerEquipment
    {
        private readonly IPowerController controller;
        protected readonly IOptionsMonitor<DeviceSetting> setting;

        public PowerEquipment(IPowerController controller, IOptionsMonitor<DeviceSetting> setting)
        {
            this.controller = controller;
            this.setting = setting;
        }

        public DateTime StartTime { get; private set; }
        public DateTime StopTime { get; private set; }

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
                return controller.GetPowerStatus(index);
            }
        }

        public virtual void TurnOn()
        {
            var index = PortIndex(setting.CurrentValue);
            controller.Turn(index, Power.On);
            StartTime = DateTime.Now;
        }

        public virtual void TurnOff()
        {
            var index = PortIndex(setting.CurrentValue);
            controller.Turn(index, Power.Off);
            StopTime = DateTime.Now;
        }
    }
}