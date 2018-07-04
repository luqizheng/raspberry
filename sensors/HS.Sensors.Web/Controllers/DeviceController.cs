using HY.IO.Ports;
using Microsoft.AspNetCore.Mvc;

namespace HS.Sensors.Web.Controllers
{
    public class EqipmentParameter
    {
        public Power Power { get; set; }


    }
    public class DeviceController : Controller
    {
        private readonly GarbageTerminal terminal;
        private readonly IPowerController powerController;

        public DeviceController(GarbageTerminal processor, IPowerController powerController)
        {
            this.terminal = processor;
            this.powerController = powerController;
        }
        public IActionResult Device(bool enable)
        {
            return View();
        }
   
        public IActionResult PowerControllerStatus()
        {
            powerController.RefreshStatus();
            var status = new
            {
                    ExhaustMain = terminal.ExhaustMain.PowerStatus,
                    ExhaustSlave = terminal.ExhaustSlave.PowerStatus,
                    GrayFan = terminal.GrayFan.PowerStatus,
                    PlasmaGenerator = terminal.PlasmaGenerator.PowerStatus,
                    Pulverizer = terminal.Pulverizer.PowerStatus,
                    Pump = terminal.Pump.PowerStatus,
                    Transfer= terminal.Transfer.PowerStatus
            };
            return Ok(status);
        }
        [HttpPost]
        public IActionResult Pulverizer(EqipmentParameter enable)
        {
            if (enable.Power == Power.On)
                terminal.Pulverizer.TurnOn();
            else
                terminal.Pulverizer.TurnOff();

            return this.Ok(true);
        }
        [HttpPost]
        public IActionResult GrayFan(EqipmentParameter enable)
        {
            if (enable.Power == Power.On)
                terminal.GrayFan.TurnOn();
            else
                terminal.GrayFan.TurnOff();

            return this.Ok(true);
        }
        [HttpPost]
        public IActionResult Pump(EqipmentParameter enable)
        {
            if (enable.Power == Power.On)
                terminal.Pump.TurnOn();
            else
                terminal.Pump.TurnOff();
            return this.Ok(true);
        }

        [HttpPost]
        public IActionResult ExhaustMain(EqipmentParameter enable)
        {
            if (enable.Power == Power.On)
                terminal.ExhaustMain.TurnOn();
            else
                terminal.ExhaustMain.TurnOff();

            return this.Ok(true);
        }
        [HttpPost]
        public IActionResult ExhaustSlave(EqipmentParameter enable)
        {
            if (enable.Power == Power.On)
                terminal.ExhaustSlave.TurnOn();
            else
                terminal.ExhaustSlave.TurnOff();

            return this.Ok(true);
        }
        [HttpPost]
        public IActionResult PlasmaGenerator(EqipmentParameter enable)
        {
            if (enable.Power == Power.On)
                terminal.PlasmaGenerator.TurnOn();
            else
                terminal.PlasmaGenerator.TurnOff();

            return this.Ok(true);
        }
    }
}