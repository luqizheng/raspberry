﻿using HY.IO.Ports.Extentions;

namespace HY.IO.Ports
{
    public class Pulverizer : Equipment
    {
        public Pulverizer(IPowerController controller, EquipmentSetting setting) : base(controller, setting)
        {
        }

        protected override int PortIndex(EquipmentSetting setting)
        {
            return setting.Pulverizer;
        }
    }



}
