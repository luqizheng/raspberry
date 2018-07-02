using System;
using System.Collections.Generic;
using System.Text;

namespace HY.IO.Ports
{

    public enum Power
    {
        On, Off
    }

    public interface IPowerController
    {
        /// <summary>
        /// 打开
        /// </summary>
        /// <param name="onOroff"></param>
        /// <returns></returns>
        bool Turn(int portIndex, Power setToStatus);
        /// <summary>
        /// 状态
        /// </summary>
        Power GetPowerStatus(int portIndex);

    }
    public enum Speed
    {
        Slow,
        Normal,
        Fast,
      
    }
    public interface ITransmissionController : IPowerController
    {
        /// <summary>
        /// 越大 越快
        /// </summary>
        /// <param name="speed"></param>
        void Adjst(Speed speed);
    }

}
