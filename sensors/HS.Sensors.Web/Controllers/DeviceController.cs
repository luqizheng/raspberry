﻿using HY.IO.Ports;
using Microsoft.AspNetCore.Mvc;

namespace HS.Sensors.Web.Controllers
{
    public class DeviceController : Controller
    {
        private readonly GarbageTerminal terminal;
        private readonly IPowerController powerController;

        /// <summary>
        ///
        /// </summary>
        /// <param name="processor"></param>
        /// <param name="powerController"></param>
        public DeviceController(GarbageTerminal processor, IPowerController powerController)
        {
            this.terminal = processor;
            this.powerController = powerController;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
        public IActionResult Device(bool enable)
        {
            return View();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
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
                Transfer = terminal.Transfer.PowerStatus
            };
            return Ok(status);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enable"></param>
        /// <returns></returns>
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

        [HttpPost]
        public IActionResult Transfer(EqipmentParameter enable)
        {
            if (enable.Power == Power.On)
                terminal.Transfer.TurnOn();
            else
                terminal.Transfer.TurnOff();

            return this.Ok(true);
        }

        [HttpGet]
        public IActionResult TurnOff()
        {
            terminal.TurnOff();
            return this.Ok(true);
        }

        [HttpGet]
        public IActionResult TurnOn()
        {
            try
            {
                terminal.TurnOn();
                return this.Ok(new
                {
                    success = false,
                    message = "启动中",
                });
            }
            catch (TerminalException ex)
            {
                return this.BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpPost]
        public IActionResult StartTransfer(TransferParameter doNotStop)
        {
            try
            {
                terminal.StartTransfer(doNotStop);
                return this.Ok(new { success = true, message = "启动成功" });
            }
            catch (TerminalException ex)
            {
                return this.BadRequest(new { success = true, message = ex.Message });
            }
        }

        [HttpGet()]
        public IActionResult StopTransfer()
        {
            try
            {
                terminal.StopTransfer();
                return this.Ok(new { success = true, message = "停止传输器成功" });
            }
            catch (TerminalException ex)
            {
                return this.BadRequest(new { success = true, message = ex.Message });
            }
        }
    }
}