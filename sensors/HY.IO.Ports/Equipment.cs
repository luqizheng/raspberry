using HY.IO.Ports.Extentions;

namespace HY.IO.Ports
{
    public abstract class Equipment
    {
        private readonly IPowerController controller;
        private readonly Extentions.EquipmentSetting setting;

        public Equipment(IPowerController controller, EquipmentSetting setting)
        {
            this.controller = controller;
            this.setting = setting;
        }
        protected abstract int PortIndex(EquipmentSetting setting);

        public int PositionIndex { get; set; }

        public Power PowerStatus
        {
            get
            {
                return controller[PositionIndex];
            }
        }
        public void TurnOn()
        {
            controller.Turn(setting.Pulverizer, Power.On);
        }

        public void TurnOff()
        {
            controller.Turn(setting.Pulverizer, Power.Off);
        }
    }
}
