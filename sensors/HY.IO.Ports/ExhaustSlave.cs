using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;

namespace HY.IO.Ports
{
    public class ExhaustSlave : PowerEquipment
    {
        public ExhaustSlave(IPowerController controller, IOptionsMonitor<DeviceSetting> setting) : base(controller, setting)
        {
            Controller = controller;
        }

        public IPowerController Controller { get; }

        protected override int PortIndex(DeviceSetting setting)
        {
            return setting.PowerControllerSetting.ExhaustSlave;
        }
    }
}