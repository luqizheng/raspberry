using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;

namespace HY.IO.Ports
{
    public class UVLight : PowerEquipment
    {
        public UVLight(IPowerController controller, IOptionsMonitor<DeviceSetting> setting) : base(controller, setting)
        {
        }

        protected override int PortIndex(DeviceSetting setting)
        {
            return setting.PowerControllerSetting.UVLight;
        }
    }
}