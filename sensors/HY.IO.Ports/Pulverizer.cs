using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;

namespace HY.IO.Ports
{
    public class Pulverizer : PowerEquipment
    {
        public Pulverizer(IPowerController controller, IOptionsMonitor<DeviceSetting> setting)
            : base(controller, setting)
        {
        }

        protected override int PortIndex(DeviceSetting setting)
        {
            return setting.PowerControllerSetting.Pulverizer;
        }
    }
}