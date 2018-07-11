using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;

namespace HY.IO.Ports
{
    public class ExhaustMain : Equipment
    {
        public ExhaustMain(IPowerController controller, IOptionsMonitor<DeviceSetting> setting) : base(controller, setting)
        {
            Controller = controller;
        }

        public IPowerController Controller { get; }

        /// <summary>
        ///
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        protected override int PortIndex(DeviceSetting setting)
        {
            return setting.PowerControllerSetting.ExhaustMain;
        }
    }
}