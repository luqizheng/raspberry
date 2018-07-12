using HY.IO.Ports;

namespace HS.Sensors.Web.Controllers
{
    public class EqipmentParameter
    {
        public Power Power { get; set; }
    }

    public class DevicePowerPameter
    {
        public int PowerIndex { get; set; }
        public string EquipmentName { get; set; }
    }

    public class DeviceSensorSetting
    {
        public int SensorIndex { get; set; }
        public string SensorName { get; set; }
    }
}