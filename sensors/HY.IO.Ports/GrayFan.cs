using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;
using System;
using System.Threading;

namespace HY.IO.Ports
{
    public class GrayFan : PowerEquipment
    {
        private GrayFanTimeInfo grayFanTimeInfo = new GrayFanTimeInfo();
        private Timer timer;

        public GrayFan(IPowerController controller, IOptionsMonitor<DeviceSetting> setting) : base(controller, setting)
        {
            timer = new Timer(GrayFanTimer, null, 5000, 5000);
        }

        private void GrayFanTimer(object state)
        {
            var poweOn = this.PowerStatus == Power.On;

            if (Terminal.Enable == false)
            {
                if (poweOn)
                    this.TurnOff();
                return;
            }
            if (poweOn)
            {
                var remind = DateTime.Now - (this.grayFanTimeInfo.RunTime ?? DateTime.Now);
                if (remind.TotalSeconds > this.setting.CurrentValue.GrayFanRuntime.GrayFanRunSeconds)
                {
                    this.TurnOff();
                }
            }
            else
            {
                if (this.grayFanTimeInfo.Sleep == null)
                {
                    this.TurnOn();
                    return;
                }
                var remind = DateTime.Now - (this.grayFanTimeInfo.Sleep ?? DateTime.Now);
                if (remind.TotalSeconds > this.setting.CurrentValue.GrayFanRuntime.GrayFanRunSeconds)
                {
                    this.TurnOn();
                }
            }
        }

        public GarbageTerminal Terminal { get; internal set; }

        protected override int PortIndex(DeviceSetting setting)
        {
            return setting.PowerControllerSetting.GrayFan;
        }

        public override void TurnOff()
        {
            base.TurnOff();
            grayFanTimeInfo.Sleep = DateTime.Now;
        }

        public override void TurnOn()
        {
            base.TurnOn();
            grayFanTimeInfo.RunTime = DateTime.Now;
        }
    }
}