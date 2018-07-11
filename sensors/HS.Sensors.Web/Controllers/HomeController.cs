using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using HS.Sensors.Web.Models;
using HY.IO.Ports.Extentions;
using Microsoft.CodeAnalysis.Options;
using Microsoft.Extensions.Options;
using HY.IO.Ports;

namespace HS.Sensors.Web.Controllers
{
    public class HomeController : Controller
    {
        public GarbageTerminal Terminal { get; }

        public HomeController(GarbageTerminal terminal)
        {
            Terminal = terminal;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail()
        {
            return View();
        }

        public IActionResult Setting()
        {
            return View();
        }

        public IActionResult Reboot()
        {
            if (Terminal.Enable)
            {
                Terminal.GrayFan.TurnOn();
                Terminal.Pulverizer.TurnOff();
                Terminal.Transfer.TurnOff();
                Terminal.PlasmaGenerator.TurnOn();
                Terminal.ExhaustMain.TurnOn();
                Terminal.ExhaustSlave.TurnOn();
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