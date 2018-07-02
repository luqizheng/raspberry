using HY.IO.Ports.Extentions;

namespace HY.IO.Ports
{
    public class Pump : Equipment
    {
        public Pump(IPowerController controller, EquipmentSetting setting) : base(controller, setting)
        {
        }

        protected override int PortIndex(EquipmentSetting setting)
        {
            return setting.Pump;
        }
    }

   
}
