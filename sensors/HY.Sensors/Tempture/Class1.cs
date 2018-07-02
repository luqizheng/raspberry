
using System;
using Unosquare.RaspberryIO;

namespace HY.Sensors
{
    public enum SensorType
    {
        DHT11,
        DHT22,
        DHT21,
    }
    public class Class1
    {
        public Class1()
        {

        }

        public void Test()
        {
            Unosquare.RaspberryIO.Gpio.GpioPin c = Pi.Gpio[Unosquare.RaspberryIO.Gpio.WiringPiPin.Pin03];



        }
    }
}
