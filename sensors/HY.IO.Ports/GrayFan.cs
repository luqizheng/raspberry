using HY.IO.Ports.Extentions;

namespace HY.IO.Ports
{
    public class GrayFan : Equipment
    {
        public GrayFan(IPowerController controller, EquipmentSetting setting) : base(controller, setting)
        {
        }

        protected override int PortIndex(EquipmentSetting setting)
        {
            return setting.GrayFan;
        }
    }
}
