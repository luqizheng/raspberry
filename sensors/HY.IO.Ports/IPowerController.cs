using System;
using System.Collections.Generic;
using System.Text;

namespace HY.IO.Ports
{

    public enum Power
    {
        Off = 0,
        On,
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
        void RefreshStatus();

        Power this[int portIndex]
        {
            get;
        }

    }
    public enum Speed
    {
        Slow,
        Normal,
        Fast,

    }

}
