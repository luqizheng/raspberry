﻿using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;

namespace HY.IO.Ports
{
    public class ExhaustSlave : Equipment
    {
        public ExhaustSlave(ITransmissionController controller, IOptionsMonitor<DeviceSetting> setting) : base(controller, setting)
        {
            Controller = controller;
        }

        public ITransmissionController Controller { get; }

        protected override int PortIndex(DeviceSetting setting)
        {
            return setting.PowerControllerSetting.ExhaustMain;
        }
    }
}