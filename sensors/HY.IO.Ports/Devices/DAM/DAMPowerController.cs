using System;

namespace HY.IO.Ports.Devices.DAM
{
    /// <summary>
    ///
    /// </summary>
    public class DAMPowerController : IPowerController, IOpenCloseController
    {
        public DAMPowerController(DAM damDevice)
        {
            DamController = damDevice ?? throw new ArgumentNullException(nameof(damDevice));
            DamController.OptocouplerPortsChanged += DamController_OptocouplerPortsChanged;
        }

        public event EventHandler<OpenCloseEventArgs> OnOptocouplerChange;

        /// <summary>
        ///
        /// </summary>
        public DAM DamController { get; }

        public Power GetPowerStatus(int portIndex)
        {
            if (DamController.RelayPort.ContainsKey(portIndex))
                return this.DamController.RelayPort[portIndex] ? Power.On : Power.Off;
            return Power.Off;
        }

        public bool IsOpen(int portIndex)
        {
            return this.DamController.OptocouplerPort[portIndex];
        }

        /// <summary>
        /// 刷新 控制器所有端口的状态
        /// </summary>
        /// <returns></returns>
        public void RefreshStatus()
        {
            DamController.RefreshRelayStatus();
            DamController.RefreshOptocouplerStatus();
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

        private void DamController_OptocouplerPortsChanged(object sender, DAMEventArgs e)
        {
            if (OnOptocouplerChange != null)
            {
                foreach (var item in e.StatusChanged)
                {
                    OnOptocouplerChange.Invoke(this, new OpenCloseEventArgs()
                    {
                        PortIndex = item.Key,
                        IsOpen = item.Value
                    });
                }
            }
        }
    }
}