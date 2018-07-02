using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace HY.IO.Ports.Extentions
{
    public class EquipmentSetting
    {
        public string PowerControllerComPath { get; set; }
        public int Pulverizer { get; set; }
        public int GrayFan { get; set; }
        public int Pump { get; set; }
        //  public int Transfer { get; set; }
        public int ExhaustMain { get; set; }
        public int ExhaustSlave { get; set; }
        public int PlasmaGenerator { get; set; }

    }
    public static class DeviceExtentions
    {

        public static IServiceCollection AddControllerWatch<T>(this IServiceCollection service, EquipmentSetting s)
            where T : IPowerController
        {


            service.AddSingleton<GrayFan>();
            service.AddSingleton<Pulverizer>();
            service.AddSingleton<Pump>();
            service.AddSingleton<ExhaustSlave>();
            service.AddSingleton<ExhaustMain>();
            service.AddSingleton<PlasmaGenerator>();
            return service;

        }
    }
}
