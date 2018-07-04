﻿using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;

namespace HY.IO.Ports
{
    public abstract class Equipment
    {
        private readonly IPowerController controller;
        private readonly IOptionsMonitor<Extentions.DeviceSetting> setting;

        public Equipment(IPowerController controller, IOptionsMonitor<DeviceSetting> setting)
        {
            this.controller = controller;
            this.setting = setting;
        }
        protected abstract int PortIndex(DeviceSetting setting);

        public int PositionIndex { get; set; }

        public Power PowerStatus
        {
            get
            {
                return controller[PositionIndex];
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
