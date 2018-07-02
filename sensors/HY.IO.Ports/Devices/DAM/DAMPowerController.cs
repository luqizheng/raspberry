using System;
using System.Collections.Generic;
using System.Text;
namespace HY.IO.Ports.Devices.DAM
{
    /// <summary>
    /// 
    /// </summary>
    public class DAMPowerController : IPowerController
    {

        public DAMPowerController(DAM damDevice)
        {
            DamController = damDevice ?? throw new ArgumentNullException(nameof(damDevice));
        }
        /// <summary>
        /// 
        /// </summary>
        public DAM DamController { get; }


        public Power this[int portIndex]
        {
            get
            {
                return this.DamController.RelayPort[portIndex] ? Power.On : Power.Off;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="portIndex"></param>
        /// <param name="setToStatus"></param>
        /// <returns></returns>
        public bool Turn(int portIndex, Power setToStatus)
        {
            if (setToStatus == Power.On)
                return DamController.Open(portIndex);
            else
                return DamController.Close(portIndex);
        }
        /// <summary>
        /// 刷新 控制器所有端口的状态
        /// </summary>
        /// <returns></returns>
        public void RefreshStatus()
        {
            DamController.RefreshRelayStatus();


        }
    }
}
