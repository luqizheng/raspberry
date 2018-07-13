using HY.IO.Ports.Devices;
using Microsoft.Extensions.DependencyInjection;

namespace HY.IO.Ports.Extentions
{
    public static class DeviceExtentions
    {
        public static IServiceCollection AddControllerWatch<T, T1>(this IServiceCollection service)
            where T : class, IPowerController
            where T1 : class, IOpenCloseController
        {
            service.AddSingleton<ITransmissionController, TransmissionController>();//jsut debug
            service.AddSingleton<IPowerController, T>();
            service.AddSingleton<IOpenCloseController, T1>();
            service.AddTransient<GrayFan>();
            service.AddTransient<Pulverizer>();
            service.AddTransient<PrimaryPump>();
            service.AddTransient<SecondaryPump>();
            service.AddTransient<ExhaustSlave>();
            service.AddTransient<ExhaustMain>();
            service.AddTransient<PrimaryPlasmaGenerator>();
            service.AddTransient<SecondaryPlasmaGenerator>();
            service.AddTransient<PlasmaGeneratorGroup>();//等离子组合
            service.AddTransient<Transfer>();
            service.AddSingleton<GarbageTerminal>();
            service.AddTransient<ReactionCabin>();
            service.AddTransient<UVLight>();
            return service;
        }
    }
}