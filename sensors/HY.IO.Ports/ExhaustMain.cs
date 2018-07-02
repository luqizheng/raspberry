using HY.IO.Ports.Extentions;

namespace HY.IO.Ports
{
    public class ExhaustMain : Equipment
    {
        public ExhaustMain(ITransmissionController controller, EquipmentSetting setting) : base(controller, setting)
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
