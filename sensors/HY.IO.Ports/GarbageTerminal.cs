using System;
using System.Threading;
using System.Threading.Tasks;

namespace HY.IO.Ports
{
    public class GarbageTerminal : IDisposable
    {
        private CancellationTokenSource terminalTurnOnTask = new CancellationTokenSource();
        private CancellationTokenSource terminalTurnOffTask = new CancellationTokenSource();
        private Task turnOnTask;
        private Task turnOffTask;
        private bool isRunning = false;
        private Timer timer;
        private readonly IPowerController controller;

        public GarbageTerminal(Pulverizer pulverizer,
            GrayFan grayFan, Pump pump, PlasmaGenerator plasmaGenerator,
             ExhaustMain exhaustMain, ExhaustSlave exhaustSlave, Transfer transfer,
             ReactionCabin reactionCabin,
             IPowerController controller)
        {
            Pulverizer = pulverizer;
            GrayFan = grayFan;
            this.Pump = pump;
            PlasmaGenerator = plasmaGenerator;
            ExhaustMain = exhaustMain;
            ExhaustSlave = exhaustSlave;
            Transfer = transfer;
            ReactionCabin = reactionCabin;
            this.controller = controller;
            timer = new Timer(GetStatus, null, 1000, 200);

            this.GrayFan.Terminal = this;
        }

        private void GetStatus(object state)
        {
            if (isRunning)
                return;
            isRunning = true;
            controller.RefreshStatus();

            StatusRefreched?.Invoke(this, EventArgs.Empty);
            isRunning = false;
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
                    || PlasmaGenerator.PowerStatus == Power.On
                    || ExhaustMain.PowerStatus == Power.On
                    || ExhaustSlave.PowerStatus == Power.On;
            }
        }

        public bool TransferModelEnable
        {
            get
            {
                return Pulverizer.PowerStatus == Power.On || Transfer.PowerStatus == Power.On;
            }
        }

        /// <summary>
        ///
        /// </summary>
        public void TurnOn()
        {
            if (turnOffTask != null && !turnOffTask.IsCompleted && Enable)
            {
                throw new TerminalException("正在关闭中，请等完全关闭后再启动.");
            }

            var token = terminalTurnOnTask.Token;

            turnOnTask = Task.Run(() =>
                 {
                     //无论设备么情况，传输和碎料必须停止
                     this.Pulverizer.TurnOff();
                     Thread.Sleep(200);
                     this.Transfer.TurnOff();

                     Pump.TurnOn();
                     Thread.Sleep(5000);
                     PlasmaGenerator.TurnOn();
                     Thread.Sleep(3000);
                     ExhaustMain.TurnOn();
                     Thread.Sleep(200);
                     ExhaustSlave.TurnOn();

                     this.GrayFan.TurnOn();
                 }, token);
        }

        public Task TurnOff()
        {
            if (turnOnTask != null && !turnOnTask.IsCompleted)
            {
                terminalTurnOnTask.Cancel();
            }

            //无论设备么情况，传输和碎料必须停止
            this.Pulverizer.TurnOff();
            this.Transfer.TurnOff();

            var token = terminalTurnOffTask.Token;

            return turnOffTask = Task.Run(() =>
               {
                   ExhaustMain.TurnOff();
                   Thread.Sleep(200);
                   ExhaustSlave.TurnOff();
                   Thread.Sleep(10000);
                   Pump.TurnOff();
                   Thread.Sleep(200);
                   PlasmaGenerator.TurnOff();

                   this.GrayFan.TurnOff();
               }, token);
        }

        private CancellationTokenSource _transferTokenSource = new CancellationTokenSource();
        private CancellationToken _transferEquipment;
        private Task TransferEquipmentRunningTask = null;

        public void StartTransfer(TransferParameter transfer)
        {
            if (!this.Enable)

                throw new TerminalException("还没启动，请启动后再执行传输");
            _transferTokenSource = new CancellationTokenSource(transfer.RunningSeconds * 1000);
            _transferEquipment = _transferTokenSource.Token;

            TransferEquipmentRunningTask = Task.Run(() =>
             {
                 Pulverizer.TurnOn();
                 Thread.Sleep(5 * 1000);
                 Transfer.TurnOn();
                 var startTime = DateTime.Now;
                 var now = new TimeSpan(0, 0, 0);
                 while (now.TotalSeconds < transfer.RunningSeconds)
                 {
                     Thread.Sleep(2000);
                     now = (DateTime.Now - startTime);
                 }

                 Pulverizer.TurnOff();

                 this.Transfer.TurnOff();
             }, _transferEquipment);
        }

        public void StopTransfer()
        {
            Task.Run(() =>
            {
                this.Pulverizer.TurnOff();
                this.Transfer.TurnOff();
            });
        }

        public void Dispose()
        {
            TurnOff();
        }

        public Transfer Transfer { get; set; }
        public ReactionCabin ReactionCabin { get; }

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