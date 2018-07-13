using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;

namespace HY.IO.Ports
{
    public class PrimaryPlasmaGenerator : PowerEquipment
    {
        public PrimaryPlasmaGenerator(IPowerController controller, IOptionsMonitor<DeviceSetting> setting) : base(controller, setting)
        {
        }

        protected override int PortIndex(DeviceSetting setting)
        {
            return setting.PowerControllerSetting.PrimaryPlasmaGenerator;
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
    }
}