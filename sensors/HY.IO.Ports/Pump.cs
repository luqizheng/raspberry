using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;

namespace HY.IO.Ports
{
    public class PrimaryPump : PowerEquipment
    {
        public PrimaryPump(IPowerController controller, IOptionsMonitor<DeviceSetting> setting) : base(controller, setting)
        {
        }

        protected override int PortIndex(DeviceSetting setting)
        {
            return setting.PowerControllerSetting.PrimaryPump;
        }
    }

    public class SecondaryPump : PowerEquipment
    {
        public SecondaryPump(IPowerController controller, IOptionsMonitor<DeviceSetting> setting) : base(controller, setting)
        {
        }

        protected override int PortIndex(DeviceSetting setting)
        {
            return setting.PowerControllerSetting.SecondaryPump;
        }
    }
}