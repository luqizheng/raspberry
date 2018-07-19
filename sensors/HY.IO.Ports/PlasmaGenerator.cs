using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;
using System;
using System.Threading;

namespace HY.IO.Ports
{
    public class PlasmaGeneratorGroup : IPowerEquipment

    {
        private Timer plasmaGeneratorTimer;
        private DateTime startTime;
        private DateTime stopTime;

        private readonly DeviceSetting setting;

        //private DateTime plasmaGeneoratorStartTime;

        public PlasmaGeneratorGroup(PrimaryPlasmaGenerator primary, SecondaryPlasmaGenerator second, DeviceSetting setting)
        {
            Primary = primary;
            Second = second;
            this.setting = setting;
            plasmaGeneratorTimer = new Timer(switchEq, null, 10000, 10000);
        }

        private void switchEq(object state)
        {
            if (Terminal == null || Terminal.Enable == false && !Terminal.TurnOffing)
            {
                return;
            }
            if (Primary.Overload)
            {
                Primary.TurnOff();
                Second.TurnOn();
            }
            else if (Second.Overload)
            {
                Second.TurnOff();
                Primary.TurnOn();
            }
        }

        public GarbageTerminal Terminal { get; set; }

        public void TurnOn()
        {
            startTime = DateTime.Now;

            if (!Primary.Overload)
            {
                Primary.TurnOn();
            }
            else
            {
                Second.TurnOn();
            }
        }

        public void TurnOff()
        {
            stopTime = DateTime.Now;
            if (Primary.PowerStatus == Power.On)
            {
                Primary.TurnOff();
            }
            if (Second.PowerStatus == Power.On)
            {
                Second.TurnOff();
            }
        }

        public PrimaryPlasmaGenerator Primary { get; }
        public SecondaryPlasmaGenerator Second { get; }

        public DateTime StartTime => startTime;

        public DateTime StopTime => stopTime;

        public Power PowerStatus => Primary.PowerStatus == Power.On || Second.PowerStatus == Power.On ? Power.On : Power.Off;
    }

    public class PrimaryPlasmaGenerator : PowerEquipment
    {
        public PrimaryPlasmaGenerator(IPowerController controller, IOptionsMonitor<DeviceSetting> setting) : base(controller, setting)
        {
        }

        protected override int PortIndex(DeviceSetting setting)
        {
            return setting.PowerControllerSetting.PrimaryPlasmaGenerator;
        }

        public bool Overload
        {
            get
            {
                return this.PowerStatus == Power.On && (DateTime.Now - StartTime).TotalSeconds > this.setting.CurrentValue.PlasmaRuntime.SwitchTimeSeconds;
            }
        }
    }

    public class SecondaryPlasmaGenerator : PowerEquipment
    {
        public SecondaryPlasmaGenerator(IPowerController controller, IOptionsMonitor<DeviceSetting> setting) : base(controller, setting)
        {
        }

        protected override int PortIndex(DeviceSetting setting)
        {
            return setting.PowerControllerSetting.SecondaryPlasmaGenerator;
        }

        public bool Overload
        {
            get
            {
                return this.PowerStatus == Power.On && (DateTime.Now - StartTime).TotalSeconds > 60 * 20;
            }
        }
    }
}