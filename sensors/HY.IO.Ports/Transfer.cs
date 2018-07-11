using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;

namespace HY.IO.Ports
{
    public class Transfer : Equipment
    {
        public Transfer(IPowerController controller, IOptionsMonitor<DeviceSetting> setting) : base(controller, setting)
        {
        }

        protected override int PortIndex(DeviceSetting setting)
        {
            return setting.PowerControllerSetting.Transfer;
        }
    }
}