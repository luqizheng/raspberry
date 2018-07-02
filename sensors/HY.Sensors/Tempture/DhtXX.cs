using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Unosquare.RaspberryIO;
using Unosquare.RaspberryIO.Gpio;

namespace HY.Sensors.Tempture
{
    public class SensorReadTimeoutException : Exception
    {
        public SensorReadTimeoutException(string message) : base(message)
        {

        }
    }
    /// <summary>
    /// 参考 https://cdn-shop.adafruit.com/datasheets/DHT11-chinese.pdf
    /// </summary>
    public class DhtXX
    {
        private const long MicroSeondsInTicket = 1000l;
        private readonly Stopwatch waitTimeout = new Stopwatch();
        private readonly GpioPin pin;
        private readonly SensorType _sensorType;

        public DhtXX(int pin, SensorType types)
        {
            this.pin = Pi.Gpio[P1.Gpio04];
            //初始化感应器。
            //this.pin.PinMode = GpioPinDriveMode.Output;
            //this.pin.WriteAsync(GpioPinValue.High);

        }


        /// <summary>
        /// Reads the temperature.
        /// </summary>
        /// <param name="S">if set to <c>true</c> [s].</param>
        /// <returns>Task&lt;System.Single&gt;.</returns>
        public TempertureAndHumidity Read()
        {


            Console.WriteLine("初始化状态:" + pin.ReadValue());
            //只是初始化。构造函数那边做了，只是再次确保执行正确
            //pin.PinMode = GpioPinDriveMode.Output;
            //pin.Write(GpioPinValue.High);
            //Console.WriteLine("初始化，拉高电平1秒");
            Thread.Sleep(1000);
            //开始和传感器交互，等待交互数据。
            WaitForRead();
            //ReadData(out var temp, out var hum);
            return new TempertureAndHumidity()
            {
                Humidity = 0,
                Temperture = 0
            };

        }

        private void ReadData(out float template, out float hum)
        {
            pin.PinMode = GpioPinDriveMode.Input;

            var _data = new int[0];
            for (int i = 0; i < 40; ++i)
            {
                //Console.WriteLine("读数据1 ");
                var lowCycles = 0;
                var highCycles = 0;
                //Console.WriteLine(pin.ReadValue());

                var c = pin.ReadValue();
                while (c == GpioPinValue.Low)
                {
                    lowCycles++;
                    c = pin.ReadValue();
                }
                // Console.WriteLine("low count:" + lowCycles);
                while (pin.ReadValue() == GpioPinValue.High)
                {
                    highCycles++;
                }
                //var lowCycles = GetPingLevelTimeTicket(GpioPinValue.Low);

                //var highCycles = GetPingLevelTimeTicket(GpioPinValue.High);
                //Console.WriteLine("high:{0},low{1},{2}", highCycles, lowCycles, highCycles > lowCycles ? 1 : 0);
                _data[i / 8] <<= 1;

                if (highCycles > lowCycles)
                {
                    _data[i / 8] |= 1;
                }

            }
            //TIME CRITICAL_END #############
            // Check we read 40 bits and that the checksum matches.
            if (_data[4] == ((_data[0] + _data[1] + _data[2] + _data[3]) & 0xFF))
            {
                throw new ArgumentException("校检失败");
            }

            template = Convert.ToSingle(_data[2]);
            hum = Convert.ToSingle(_data[0]);
        }

        private void WaitForRead()
        {
            long data = 0;
            //var list = new bool[40];
            //总线空闲状态为高电平,主机把总线拉低等待DHT11响应,主机把总线拉低必
            //须大于18毫秒,保证DHT11能检测到起始信号。
            pin.PinMode = GpioPinDriveMode.Output;
            //Console.WriteLine("输出模式 拉低pin脚18ms以上，发送开始信号: ");
            pin.Write(GpioPinValue.Low);

            Thread.Sleep(20);

            pin.PinMode = GpioPinDriveMode.Output;
            //Console.WriteLine("发送一个high");
            pin.Write(GpioPinValue.High);//主机告诉：我已经准备好 接受数据了
            pin.PinMode = GpioPinDriveMode.Input;
            //Console.WriteLine("等待一个high");
            while (pin.ReadValue() == GpioPinValue.Low)
                continue;
            //Console.WriteLine("等待一个hight，成功");
            while (pin.ReadValue() == GpioPinValue.High)
                continue;

            for (int i = 0; i < 40; ++i)
            {
                // Console.WriteLine(i);
                //Console.WriteLine("读数据1 ");
                var lowCycles = 0;
                var highCycles = 0;
                //Console.WriteLine(pin.ReadValue());

                var c = pin.ReadValue();
                while (c == GpioPinValue.Low)
                {
                    lowCycles++;
                    c = pin.ReadValue();
                }
                //Console.WriteLine("low count:" + lowCycles);
                while (pin.ReadValue() == GpioPinValue.High)
                {
                    highCycles++;
                }
                //var lowCycles = GetPingLevelTimeTicket(GpioPinValue.Low);

                //var highCycles = GetPingLevelTimeTicket(GpioPinValue.High);
                //Console.WriteLine("high:{0},low{1},{2}", highCycles, lowCycles, highCycles > lowCycles ? 1 : 0);

                data = data << 1;
                if (highCycles > lowCycles)
                {
                    data = data | 1;

                }
                else
                {
                    data = data | 0;
                }

                //Console.WriteLine(_data[i / 8]);

            }
            byte temp = Convert.ToByte((data &
                    Convert.ToInt64("1111111100000000000000000000000000000000", 2)) >> 32);
            byte humid = Convert.ToByte((data &
                Convert.ToInt64("0000000000000000111111110000000000000000", 2)) >> 16);
            byte calibr = Convert.ToByte((data & Convert.ToInt64("11111111", 2)));
            //if ((calibr ^ (temp + humid)) != 0)
            //{
            //    throw new Exception("测量数据有误 Line138");
            //}
           var  temprature = Convert.ToString(temp);
            var humidity = Convert.ToString(humid);
            var calibrate = Convert.ToString(calibr);
            Console.WriteLine(temprature + "," + humid);
            //var _data = new int[5];
            //for (int i = 0; i < list.Length; i++)
            //{
            //    _data[i % 5] <<= 1;
            //    _data[i % 5] |= list[i] ? 1 : 0;
            //}
            //if (_data[4] == ((_data[0] + _data[1] + _data[2] + _data[3]) & 0xFF))
            //{
            //    throw new ArgumentException("校检失败");
            //}
            //var humity = new byte[]
            //{
            //     0,0,_data[0], _data[1]
            //};
            //var temp = new byte[]
            //{
            // 0,0,   _data[2],_data[3]
            //}
            //foreach (var d in list)
            //{
            //    Console.Write(d);

            //}
            //Console.WriteLine();
            //foreach (var b in _data)
            //{
            //    Console.WriteLine(Convert.ToString(b));
            //}
            //Console.WriteLine(_data[0].ToString("X2") + "," + (int)_data[1] + "," + (int)_data[2] + "," + (int)_data[3]);

            //Console.WriteLine(BitConverter.ToSingle(humity, 0).ToString("0.00") + "," + BitConverter.ToSingle(temp, 0).ToString("0.00"));
        }

        private void ExpectLevel(GpioPinValue value, int milliseconds)
        {
            if (this.pin.PinMode != GpioPinDriveMode.Input)
            {
                this.pin.PinMode = GpioPinDriveMode.Input;
            }

            if (!pin.WaitForValue(value, milliseconds))
            {

                if (!pin.WaitForValue(value, milliseconds))
                    throw new SensorReadTimeoutException("等待" + value + " 读取DHT11 温度和湿度超时");
            }
        }

        private long GetPingLevelTimeTicket(GpioPinValue value)
        {

            var now = DateTime.Now.Ticks;

            while (pin.ReadValue() == value)
            {

                continue;
                // Console.WriteLine("in data"+pin.ReadValue());
            }
            return DateTime.Now.Ticks - now;
        }


    }

    public class TempertureAndHumidity
    {
        public float Temperture { get; set; }
        public float Humidity { get; set; }
    }
}
