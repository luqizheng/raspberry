namespace HY.IO.Ports
{
    public interface ITransmissionController : IPowerController
    {
        /// <summary>
        /// 越大 越快
        /// </summary>
        /// <param name="speed"></param>
        void Adjst(Speed speed);
    }
}