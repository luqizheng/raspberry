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

        public DAMPowerController(DAM damController)
        {
            DamController = damController ?? throw new ArgumentNullException(nameof(damController));
        }
        /// <summary>
        /// 
        /// </summary>
        public DAM DamController { get; }
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
        /// 
        /// </summary>
        /// <param name="portIndex"></param>
        /// <returns></returns>
        public Power GetPowerStatus(int portIndex)
        {
            DamController.RefreshDeviceStatus();
           
            return DamController.RelayPort[portIndex] ? Power.On : Power.Off;
        }
    }
}
