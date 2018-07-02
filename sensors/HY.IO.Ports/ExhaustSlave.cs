using HY.IO.Ports.Extentions;

namespace HY.IO.Ports
{
    public class ExhaustSlave : Equipment
    {
        public ExhaustSlave(ITransmissionController controller, EquipmentSetting setting) : base(controller, setting)
        {
            Controller = controller;
        }

        public ITransmissionController Controller { get; }

        protected override int PortIndex(EquipmentSetting setting)
        {
            return setting.ExhaustMain;
        }
    }
}
