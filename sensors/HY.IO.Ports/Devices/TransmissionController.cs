using System;
using System.Collections.Generic;
using System.Text;

namespace HY.IO.Ports.Devices
{
    public class TransmissionController : ITransmissionController
    {
        public Power this[int portIndex] => throw new NotImplementedException();

        public void Adjst(Speed speed)
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
