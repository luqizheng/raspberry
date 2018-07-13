using HY.IO.Ports.Extentions;
using Microsoft.Extensions.Options;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HY.IO.Ports
{
    public class GarbageTerminal : IDisposable
    {
        private readonly IPowerController controller;
        private CancellationToken _transferEquipment;
        private CancellationTokenSource _transferTokenSource = new CancellationTokenSource();
        private CancellationTokenSource terminalTurnOffTask = new CancellationTokenSource();
        private CancellationTokenSource terminalTurnOnTask = new CancellationTokenSource();
        private Task TransferEquipmentRunningTask = null;
        private Task turnOffTask;
        private Task turnOnTask;

        public GarbageTerminal(Pulverizer pulverizer,
            GrayFan grayFan, PrimaryPump pump, PlasmaGenerator plasmaGenerator,
             ExhaustMain exhaustMain, ExhaustSlave exhaustSlave, Transfer transfer,
             ReactionCabin reactionCabin, UVLight uvLight, SecondaryPump secondary,
             IPowerController powerController, IOptionsMonitor<DeviceSetting> deviceSetting)
        {
            Pulverizer = pulverizer;
            GrayFan = grayFan;
            this.PrimaryPump = pump;
            PlasmaGenerator = plasmaGenerator;
            ExhaustMain = exhaustMain;
            ExhaustSlave = exhaustSlave;
            Transfer = transfer;
            ReactionCabin = reactionCabin;
            UVLight = uvLight;
            this.controller = powerController;
            DeviceSetting = deviceSetting;
            this.GrayFan.Terminal = this;
            this.SecondaryPump = secondary;
        }

        /// <summary>
        /// 机械是否启动
        /// </summary>
        public bool Enable
        {
            get
            {
                return PrimaryPump.PowerStatus == Power.On
                    || PlasmaGenerator.PowerStatus == Power.On
                    || ExhaustMain.PowerStatus == Power.On
                    || UVLight.PowerStatus == Power.On
                    || ExhaustSlave.PowerStatus == Power.On;
            }
        }

        /// <summary>
        /// 主风机
        /// </summary>
        public ExhaustMain ExhaustMain { get; set; }

        /// <summary>
        /// 次风机
        /// </summary>
        public ExhaustSlave ExhaustSlave { get; set; }

        /// <summary>
        /// 吹灰
        /// </summary>
        public GrayFan GrayFan { get; set; }

        /// <summary>
        /// 等离子发生器
        /// </summary>
        public PlasmaGenerator PlasmaGenerator { get; set; }

        /// <summary>
        /// 粉碎器，闸刀
        /// </summary>
        public Pulverizer Pulverizer { get; set; }

        /// <summary>
        /// 水泵1
        /// </summary>
        public PrimaryPump PrimaryPump { get; set; }

        /// <summary>
        /// 水泵2
        /// </summary>
        public SecondaryPump SecondaryPump { get; set; }

        public ReactionCabin ReactionCabin { get; }

        /// <summary>
        ///
        /// </summary>
        public UVLight UVLight { get; }

        /// <summary>
        ///
        /// </summary>
        public IOptionsMonitor<DeviceSetting> DeviceSetting { get; }

        public Transfer Transfer { get; set; }

        public bool TransferModelEnable
        {
            get
            {
                return Pulverizer.PowerStatus == Power.On || Transfer.PowerStatus == Power.On;
            }
        }

        public void Dispose()
        {
            TurnOff();
        }

        public void StartTransfer(TransferParameter transfer)
        {
            if (!this.Enable)

                throw new TerminalException("还没启动，请启动后再执行传输");
            var setting = DeviceSetting.CurrentValue.TransferRuntime;
            var pulverizerRunningTime = setting.PulverizerRuntimerSeconds;

            if (transfer.RunningSeconds > pulverizerRunningTime)
            {
                pulverizerRunningTime = transfer.RunningSeconds;
            }
            _transferTokenSource = new CancellationTokenSource(transfer.RunningSeconds * 1000);
            _transferEquipment = _transferTokenSource.Token;

            TransferEquipmentRunningTask = Task.Run(() =>
             {
                 Pulverizer.TurnOn();
                 var pulverizerStartTime = DateTime.Now;

                 Thread.Sleep(setting.TransferStartAfterPulverizerStart * 1000);

                 Transfer.TurnOn();

                 var counter = DateTime.Now - pulverizerStartTime;
                 var remindSeconds = pulverizerRunningTime - Convert.ToInt32(counter.TotalSeconds);
                 Thread.Sleep(remindSeconds * 1000);

                 Pulverizer.TurnOff();
                 Thread.Sleep(setting.TransferStopAfterPulverizerStop * 1000);

                 this.Transfer.TurnOff();
             }, _transferEquipment);
        }

        //private void WaitTime(int seond)
        //{
        //    var now = DateTime.Now;
        //    var counter = new TimeSpan(0, 0, 0);
        //    while (counter.TotalSeconds > seond)
        //    {
        //        Thread.Sleep(1000);
        //        counter = DateTime.Now - now;
        //    }
        //}

        public void StopTransfer()
        {
            Task.Run(() =>
            {
                this.Pulverizer.TurnOff();
                this.Transfer.TurnOff();
            });
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
                   PrimaryPump.TurnOff();
                   Thread.Sleep(200);
                   PlasmaGenerator.TurnOff();
                   UVLight.TurnOff();

                   this.GrayFan.TurnOff();
               }, token);
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

                     PrimaryPump.TurnOn();
                     Thread.Sleep(5000);
                     PlasmaGenerator.TurnOn();
                     UVLight.TurnOn();

                     Thread.Sleep(3000);
                     ExhaustMain.TurnOn();
                     Thread.Sleep(200);
                     ExhaustSlave.TurnOn();

                     this.GrayFan.TurnOn();
                 }, token);
        }
    }
}