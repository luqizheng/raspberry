using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HY.IO.Ports
{
    public class GarbageProcessor
    {
        public GarbageProcessor(Pulverizer pulverizer,
            GrayFan grayFan, Pump pump,
             ExhaustMain exhaustMain, ExhaustSlave exhaustSlave)
        {
            Pulverizer = pulverizer;
            GrayFan = grayFan;
            this.Pump = pump;

            ExhaustMain = exhaustMain;
            ExhaustSlave = exhaustSlave;
        }

        public void TunrOn()
        {
            Pump.TurnOn();
            Thread.Sleep(5000);
            PlasmaGenerator.TurnOn();
            Thread.Sleep(3000);
            ExhaustMain.TurnOn();
            ExhaustSlave.TurnOn();
        }

        public void TurnOff()
        {
            ExhaustMain.TurnOff();
            ExhaustSlave.TurnOff();
            Thread.Sleep(10000);
            Pump.TurnOff();
            PlasmaGenerator.TurnOff();
        }

        /// <summary>
        /// 粉碎器，闸刀
        /// </summary>
        public Pulverizer Pulverizer { get; set; }
        /// <summary>
        /// 吹灰
        /// </summary>
        public GrayFan GrayFan { get; set; }
        /// <summary>
        /// 水泵
        /// </summary>
        public Pump Pump { get; set; }

        /// <summary>
        /// 主风机
        /// </summary>
        public ExhaustMain ExhaustMain { get; set; }
        /// <summary>
        /// 次风机
        /// </summary>
        public ExhaustSlave ExhaustSlave { get; set; }
        /// <summary>
        /// 等离子发生器
        /// </summary>
        public PlasmaGenerator PlasmaGenerator { get; set; }

    }



}
