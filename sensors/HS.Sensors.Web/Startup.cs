using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HS.Sensors.Web.hub;
using HY.IO.Ports.Devices.DAM;
using HY.IO.Ports.Extentions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace HS.Sensors.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<DeviceSetting>(options => Configuration.GetSection("DeviceSetting").Bind(options));

            services.AddLogging(logging =>
            {

                logging.AddConsole();
                logging.AddDebug();
            });

            services.AddSingleton<DAM>(sp =>
            {
                var deviceSetting = sp.GetRequiredService<IOptionsMonitor<DeviceSetting>>();
                var logger = sp.GetRequiredService<ILogger<DAM0808>>();
                return new DAM0808(logger, deviceSetting.CurrentValue.PowerControllerSetting.ComPath);
            });
            services.AddControllerWatch<DAMPowerController>();
            services.AddSignalR();
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseSignalR(routes =>
                    {
                        routes.MapHub<GarbageTerminalHub>("/GarbageTerminal");
                    });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


        }
    }
}
