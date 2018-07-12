using System;

namespace HY.IO.Ports.Devices
{
    public class TransmissionController : ITransmissionController
    {
        public void Adjst(Speed speed)
        {
            throw new NotImplementedException();
        }

        public Power GetPowerStatus(int portIndex)
        {
            throw new NotImplementedException();
        }

        public void RefreshStatus()
        {
            throw new NotImplementedException();
        }

        public bool Turn(int portIndex, Power setToStatus)
        {
            throw new NotImplementedException();
        }
    }
}