﻿using HS.Sensors.Web.Models;
using HY.IO.Ports;
using HY.IO.Ports.Extentions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Diagnostics;
using System.IO;

namespace HS.Sensors.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOptionsMonitor<DeviceSetting> setting;

        public GarbageTerminal Terminal { get; }

        public HomeController(GarbageTerminal terminal, IOptionsMonitor<DeviceSetting> setting)
        {
            Terminal = terminal;
            this.setting = setting;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }

        [Authorize]
        public IActionResult Setting()
        {
            return View();
        }

        private void Save()
        {
            using (var write = new StreamWriter("device.json"))
            {
                var writer = TextWriter.Synchronized(write);
                writer.Write(JsonConvert.SerializeObject(new { DeviceSetting = setting.CurrentValue }, Formatting.Indented));
            }
        }

        [HttpPost]
        public IActionResult OpenCloseSetting(DeviceSensorSetting parameter)
        {
            switch (parameter.SensorName)
            {
                case "FullSensor":
                    setting.CurrentValue.OpenClosePortSetting.FullSensor = parameter.SensorIndex;
                    break;

                case "EmptySensor":
                    setting.CurrentValue.OpenClosePortSetting.EmptySensor = parameter.SensorIndex;
                    break;
            }
            Save();
            return Ok(true);
        }

        [HttpPost]
        public IActionResult PowerSetting(DevicePowerPameter parameter)
        {
            switch (parameter.EquipmentName)
            {
                case "Pulverizer":

                    setting.CurrentValue.PowerControllerSetting.Pulverizer = parameter.PowerIndex;
                    break;

                case "GrayFan":
                    setting.CurrentValue.PowerControllerSetting.GrayFan = parameter.PowerIndex;
                    break;

                case "PrimaryPump":
                    setting.CurrentValue.PowerControllerSetting.PrimaryPump = parameter.PowerIndex;
                    break;

                case "SecondaryPump":
                    setting.CurrentValue.PowerControllerSetting.SecondaryPump = parameter.PowerIndex;
                    break;

                case "PrimaryPlasmaGenerator":
                    setting.CurrentValue.PowerControllerSetting.PrimaryPlasmaGenerator = parameter.PowerIndex;
                    break;

                case "SecondaryPlasmaGenerator":
                    setting.CurrentValue.PowerControllerSetting.SecondaryPlasmaGenerator = parameter.PowerIndex;
                    break;

                case "ExhaustMain":
                    setting.CurrentValue.PowerControllerSetting.ExhaustMain = parameter.PowerIndex;
                    break;

                case "ExhaustSlave":
                    setting.CurrentValue.PowerControllerSetting.ExhaustSlave = parameter.PowerIndex;
                    break;

                case "Transfer":
                    setting.CurrentValue.PowerControllerSetting.Transfer = parameter.PowerIndex;
                    break;

                case "UVLight":
                    setting.CurrentValue.PowerControllerSetting.UVLight = parameter.PowerIndex;
                    break;
            }
            Save();
            return Ok("true");
        }

        [HttpPost]
        public IActionResult PlasmaGenerator(PlasmaRuntime runtime)
        {
            setting.CurrentValue.PlasmaRuntime.SwitchTimeSeconds = runtime.SwitchTimeSeconds;
            Save();
            return Ok("true");
        }

        [HttpPost]
        public IActionResult GrayFanRunTime(GrayFanRuntime runtime)
        {
            setting.CurrentValue.GrayFanRuntime.GrayFanRunSeconds = runtime.GrayFanRunSeconds;
            setting.CurrentValue.GrayFanRuntime.GrayFanSleepSeconds = runtime.GrayFanSleepSeconds;

            Save();
            return Ok("true");
        }

        [HttpPost]
        public IActionResult TransferRuntime(TransferRuntime runtime)
        {
            setting.CurrentValue.TransferRuntime.PulverizerRuntimerSeconds = runtime.PulverizerRuntimerSeconds;
            setting.CurrentValue.TransferRuntime.TransferStartAfterPulverizerStart = runtime.TransferStartAfterPulverizerStart;
            setting.CurrentValue.TransferRuntime.TransferStopAfterPulverizerStop = runtime.TransferStopAfterPulverizerStop;
            Save();
            return Ok("true");
        }

        [Authorize]
        public IActionResult Shutdown()
        {
            if (Terminal.Enable)
            {
                Terminal.GrayFan.TurnOn();
                Terminal.Pulverizer.TurnOff();
                Terminal.Transfer.TurnOff();
                Terminal.PlasmaGeneratorGroup.TurnOn();

                Terminal.UVLight.TurnOn();
                Terminal.ExhaustMain.TurnOn();
                Terminal.ExhaustSlave.TurnOn();
                Terminal.PrimaryPump.TurnOn();
                Terminal.SecondaryPump.TurnOn();
            }

            HY.IO.Ports.Helper.ShellHelper.Bash("sudo reboot now");
            return Ok("ture");
        }

        [Authorize]
        public IActionResult Reboot()
        {
            if (Terminal.Enable)
            {
                Terminal.GrayFan.TurnOn();
                Terminal.Pulverizer.TurnOff();
                Terminal.Transfer.TurnOff();
                Terminal.PlasmaGeneratorGroup.TurnOn();
                Terminal.UVLight.TurnOn();
                Terminal.ExhaustMain.TurnOn();
                Terminal.ExhaustSlave.TurnOn();
                Terminal.PrimaryPump.TurnOn();
                Terminal.SecondaryPump.TurnOn();
            }

            HY.IO.Ports.Helper.ShellHelper.Bash("sudo reboot now");
            return Ok("ture");
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}