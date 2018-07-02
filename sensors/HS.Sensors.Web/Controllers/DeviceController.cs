using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HY.IO.Ports;
using Microsoft.AspNetCore.Mvc;

namespace HS.Sensors.Web.Controllers
{
    public class DeviceController : Controller
    {
        private readonly GarbageProcessor processor;

        public DeviceController(GarbageProcessor processor)
        {
            this.processor = processor;
        }
        public IActionResult Device(bool enable)
        {
            return View();
        }

        public IActionResult Pulverizer(bool enable)
        {
            if (enable)
                processor.Pulverizer.TurnOff();
            else
                processor.Pulverizer.TurnOff();

            return this.Ok(true);
        }

        public IActionResult GrayFan(bool enable)
        {
            if (enable)
                processor.GrayFan.TurnOff();
            else
                processor.GrayFan.TurnOff();

            return this.Ok(true);
        }
        public IActionResult Pump(bool enable)
        {
            if (enable)
                processor.Pump.TurnOff();
            else
                processor.Pump.TurnOff();
            return this.Ok(true);
        }

   
        public IActionResult ExhaustMain(bool enable)
        {
            if (enable)
                processor.ExhaustMain.TurnOff();
            else
                processor.ExhaustMain.TurnOff();

            return this.Ok(true);
        }

        public IActionResult ExhaustSlave(bool enable)
        {
            if (enable)
                processor.ExhaustSlave.TurnOff();
            else
                processor.ExhaustSlave.TurnOff();

            return this.Ok(true);
        }

        public IActionResult PlasmaGenerator(bool enable)
        {
            if (enable)
                processor.PlasmaGenerator.TurnOff();
            else
                processor.PlasmaGenerator.TurnOff();

            return this.Ok(true);
        }
    }
}