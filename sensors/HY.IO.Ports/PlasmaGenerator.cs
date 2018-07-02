using HY.IO.Ports.Extentions;

namespace HY.IO.Ports
{
    public class PlasmaGenerator : Equipment
    {
        public PlasmaGenerator(IPowerController controller, EquipmentSetting setting) : base(controller, setting)
        {
        }

        protected override int PortIndex(EquipmentSetting setting)
        {
            return setting.PlasmaGenerator;
        }
    }

}
