using HY.IO.Ports.Devices;
using Microsoft.Extensions.DependencyInjection;

namespace HY.IO.Ports.Extentions
{
    public static class DeviceExtentions
    {
        public static IServiceCollection AddControllerWatch<T>(this IServiceCollection service)
            where T : class, IPowerController
        {
            service.AddSingleton<ITransmissionController, TransmissionController>();//jsut debug
            service.AddSingleton<IPowerController, T>();
            service.AddTransient<GrayFan>();
            service.AddTransient<Pulverizer>();
            service.AddTransient<Pump>();
            service.AddTransient<ExhaustSlave>();
            service.AddTransient<ExhaustMain>();
            service.AddTransient<PlasmaGenerator>();
            service.AddTransient<Transfer>();
            service.AddSingleton<GarbageTerminal>();

            return service;
        }
    }
}