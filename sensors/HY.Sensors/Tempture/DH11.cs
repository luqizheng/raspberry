using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace HY.Sensors.Tempture
{
    public class DHT11
    {
        private const int OneUs = 10;
        private bool isCapturing = false;
        //1 us per tick = (1000L*1000L) / Stopwatch.Frequency
        private long nanosecPerTick = (1000L * 1000L * 1000L) / Stopwatch.Frequency;
        private long tickPerUs = Stopwatch.Frequency / (1000L * 1000L);
        private long TickPerUs
        {
            get
            {
                return tickPerUs;
            }
        }
        private const int TIMEOUT = 500;
     
        private Stopwatch timer = new Stopwatch();
        private Stopwatch watchdog = new Stopwatch();
        private Stopwatch beginTick = new Stopwatch();
        public DHT11(int pin)
        {
            gpioController = GpioController.GetDefault();
            if (gpioController == null)
            {
                throw new Exception("GPIO初始化失败");
            }
            gpioPin = gpioController.OpenPin(pin);
            if (gpioPin == null)
            {
                throw new Exception("GPIO初始化失败");
            }
            gpioPin.SetDriveMode(GpioPinDriveMode.Output);
            gpioPin.Write(GpioPinValue.High);
        }

        public async void CaptureData(string temprature, string humidity, string calibrate, string log)
        {
            if (!isCapturing)
            {
                log = "开始测量\n";
                isCapturing = true;
                //初始：拉高电平1s，进入准备阶段
                log += "初始：拉高电平1s，进入准备阶段\n";
                watchdog.Start();
                gpioPin.SetDriveMode(GpioPinDriveMode.Output);
                gpioPin.Write(GpioPinValue.High);
                await Task.Delay(1000);
                log += Convert.ToString(watchdog.ElapsedMilliseconds) + "ms\n";
                watchdog.Reset();
                //启动：输出模式 拉低pin脚18ms以上，发送开始信号；
                log += "输出模式 拉低pin脚18ms以上，发送开始信号: ";
                gpioPin.Write(GpioPinValue.Low);
                watchdog.Start();
                await Task.Delay(18);
                log += Convert.ToString(watchdog.ElapsedTicks / TickPerUs) + "us\n";
                watchdog.Reset();
                //拉高电平20-40us后，切换为输入模式
                log += "拉高电平20-40us后: ";
                gpioPin.Write(GpioPinValue.High);
                //watchdog.Start();
                ////await Task.Delay(new TimeSpan(20 * OneUs));
                //while (true)
                //{
                //    if (watchdog.ElapsedTicks >= 20 * TickPerUs)
                //    {
                //        log += Convert.ToString(watchdog.ElapsedTicks / TickPerUs) + "us ---+  ";
                //        watchdog.Reset();
                //        break;
                //    }
                //}                
                gpioPin.SetDriveMode(GpioPinDriveMode.Input);
                watchdog.Start();
                while (gpioPin.Read() == GpioPinValue.High) ;
                log += Convert.ToString(watchdog.ElapsedTicks / TickPerUs) + "us\n";
                watchdog.Reset();
                //DHT11 响应低电平80us
                log += "DHT11 响应低电平(标准80us):";
                watchdog.Start();
                while (gpioPin.Read() == GpioPinValue.Low)
                {
                    if (watchdog.ElapsedMilliseconds > TIMEOUT)
                    {
                        throw new Exception("测量超时 " +
                            Convert.ToString(watchdog.ElapsedTicks / TickPerUs) + "us Line70");
                    }
                }
                log += Convert.ToString(watchdog.ElapsedTicks / TickPerUs) + "us\n";
                watchdog.Reset();
                //DHT11 响应高电平80us
                log += "DHT11 响应高电平(标准80us):";
                watchdog.Start();
                while (gpioPin.Read() == GpioPinValue.High)
                {
                    if (watchdog.ElapsedMilliseconds > TIMEOUT)
                    {
                        throw new Exception("测量超时" +
                            Convert.ToString(watchdog.ElapsedTicks / TickPerUs) + "us Line80");
                    }
                }
                log += Convert.ToString(watchdog.ElapsedMilliseconds / 1000) + "us\n";
                watchdog.Reset();

                //DHT11 发送数据 共40bit,高位先出 |39-38-37-++++++++-4-3-2-1-0|
                //每1bit数据以50us低电平作为开始
                //bit 0 为26~28us的高电平
                //bit 1 为70us的高电平
                //最后拉低电平50us，表示数据传输完成
                long data = 0;
                for (int bits = 0; bits < 40; bits++)
                {
                    log += "发送数据 开始标志低电平（标准50us）: ";
                    watchdog.Start();
                    //捕获信号电平变化 低->高                    
                    while (gpioPin.Read() == GpioPinValue.Low)
                    {
                        if (watchdog.ElapsedMilliseconds > TIMEOUT)
                        {
                            throw new Exception("测量超时 " +
                                Convert.ToString(watchdog.ElapsedTicks / TickPerUs) + "us Line102");
                        }
                    }
                    watchdog.Reset();
                    log += Convert.ToString(watchdog.ElapsedTicks / TickPerUs) + "us\n";
                    log += "发送数据: 第" + Convert.ToString(bits + 1) + "位,高电平时长： ";
                    timer.Start();
                    watchdog.Start();
                    while (gpioPin.Read() == GpioPinValue.High)
                    {
                        if (watchdog.ElapsedMilliseconds > TIMEOUT)
                        {
                            throw new Exception("测量超时 " +
                                Convert.ToString(watchdog.ElapsedMilliseconds / 1000) + "us Line112");
                        }
                    }
                    log += Convert.ToString(watchdog.ElapsedMilliseconds / 1000) + "us\n";
                    timer.Stop();
                    if (timer.ElapsedTicks <= 28 * TickPerUs && timer.ElapsedTicks >= 26 * TickPerUs)
                    {
                        data = data << 1 | 0;
                    }
                    else if (timer.ElapsedTicks > 30 * TickPerUs && timer.ElapsedTicks <= 75 * TickPerUs)
                    {
                        data = data << 1 | 1;
                    }
                    else
                    {
                        throw new Exception("测量失败 Line126");
                    }
                    timer.Reset();
                    watchdog.Reset();
                }
                try
                {
                    byte temp = Convert.ToByte((data &
                        Convert.ToInt64("1111111100000000000000000000000000000000", 2)) >> 32);
                    byte humid = Convert.ToByte((data &
                        Convert.ToInt64("0000000000000000111111110000000000000000", 2)) >> 16);
                    byte calibr = Convert.ToByte((data & Convert.ToInt64("11111111", 2)));
                    if ((calibr ^ (temp + humid)) != 0)
                    {
                        throw new Exception("测量数据有误 Line138");
                    }
                    temprature = Convert.ToString(temp);
                    humidity = Convert.ToString(humid);
                    calibrate = Convert.ToString(calibr);
                }
                catch (Exception err)
                {
                    if (err is OverflowException)
                    {
                        throw new Exception("测量数据溢出");
                    }
                    else throw err;
                }

                //两次测量间隔时间>1s
                await Task.Delay(1000);
                isCapturing = false;
            }//end if(!isCapturing)

        }


    }
}
