using System;

namespace HY.IO.Ports
{
    public interface IOpenCloseController
    {
        bool IsOpen(int portIndex);

        event EventHandler<OpenCloseEventArgs> OnOptocouplerChange;
    }
}