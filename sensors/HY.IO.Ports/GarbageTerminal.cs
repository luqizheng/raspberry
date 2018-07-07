using System;
using System.Threading;
using System.Threading.Tasks;

namespace HY.IO.Ports
{
    public class GarbageTerminal
    {

        CancellationTokenSource terminalTurnOnTask = new CancellationTokenSource();
        CancellationTokenSource terminalTurnOffTask = new CancellationTokenSource();
        Task turnOnTask;
        Task turnOffTask;

        Timer timer;
        private readonly IPowerController controller;

        public GarbageTerminal(Pulverizer pulverizer,
            GrayFan grayFan, Pump pump, PlasmaGenerator plasmaGenerator,
             ExhaustMain exhaustMain, ExhaustSlave exhaustSlave, Transfer transfer, IPowerController controller)
        {
            Pulverizer = pulverizer;
            GrayFan = grayFan;
            this.Pump = pump;
            PlasmaGenerator = plasmaGenerator;
            ExhaustMain = exhaustMain;
            ExhaustSlave = exhaustSlave;
            Transfer = transfer;
            this.controller = controller;
            timer = new Timer(getStatus, null, 5000, 5000);
        }

        private void getStatus(object state)
        {
            controller.RefreshStatus();
            StatusRefreched?.Invoke(this, EventArgs.Empty);
        }
        public event EventHandler StatusRefreched;
        /// <summary>
        /// 机械是否启动
        /// </summary>
        public bool Enable
        {
            get
            {
                return Pump.PowerStatus == Power.On
                    && PlasmaGenerator.PowerStatus == Power.On
                    && ExhaustMain.PowerStatus == Power.On
                    && ExhaustSlave.PowerStatus == Power.On;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public void TurnOn()
        {
            if (turnOffTask != null && !turnOffTask.IsCompleted)
            {
                throw new TerminalException("正在关闭中，请等完全关闭后再启动.");
            }
            //无论设备么情况，传输和碎料必须停止
            this.Pulverizer.TurnOff();
            this.Transfer.TurnOff();

            var token = terminalTurnOnTask.Token;

            turnOnTask = Task.Run(() =>
                 {
                     Pump.TurnOn();
                     Thread.Sleep(5000);
                     PlasmaGenerator.TurnOn();
                     Thread.Sleep(3000);
                     ExhaustMain.TurnOn();
                     ExhaustSlave.TurnOn();
                 }, token);

        }


        public void TurnOff()
        {
            if (turnOnTask != null && !turnOnTask.IsCompleted)
            {
                terminalTurnOnTask.Cancel();
            }

            //无论设备么情况，传输和碎料必须停止
            this.Pulverizer.TurnOff();
            this.Transfer.TurnOff();


            var token = terminalTurnOffTask.Token;

            turnOffTask = Task.Run(() =>
              {
                  ExhaustMain.TurnOff();
                  ExhaustSlave.TurnOff();
                  Thread.Sleep(10000);
                  Pump.TurnOff();
                  PlasmaGenerator.TurnOff();
              }, token);

        }

        public void StartTransfer(bool DoNotStop)
        {
            if (!this.Enable)
                if (turnOffTask != null && !turnOffTask.IsCompleted)
                {
                    throw new TerminalException("还没启动，请启动后再执行传输");
                }
            Task.Run(() =>
            {

                Pulverizer.TurnOn();
                Thread.Sleep(10 * 1000);
                Transfer.TurnOn();

                if (DoNotStop == false)
                {
                    Pulverizer.TurnOff();
                    Thread.Sleep(30 * 1000);
                }
                if (DoNotStop == false)
                    this.Transfer.TurnOff();

            });

        }

        public void StopTransfer()
        {
            Task.Run(() =>
            {
                this.Pulverizer.TurnOff();
                this.Transfer.TurnOff();
            });
        }


        public Transfer Transfer { get; set; }
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
