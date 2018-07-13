using HS.Sensors.Web.hub;
using HY.IO.Ports;
using HY.IO.Ports.Devices.DAM;
using HY.IO.Ports.Extentions;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading;

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
                var logger = sp.GetRequiredService<ILogger<DAM0404>>();
                return new DAM0404(logger, deviceSetting.CurrentValue.PowerControllerSetting.ComPath);
            });
            services.AddControllerWatch<DAMPowerController, DAMPowerController>();
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
            services.AddMvc();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            }).AddCookie();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseAuthentication();
            app.UseSignalR(routes =>
            {
                routes.MapHub<GarbageTerminalHub>("/GarbageTerminal");
            });

            if (env.IsDevelopment())
            {
                //app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            timer = new Timer(Send, app.ApplicationServices, 2000, 2000);
        }

        private Timer timer;

        private void Send(object state)
        {
            var context = (ServiceProvider)state;
            var terminal = context.GetService<GarbageTerminal>();
            var status = new
            {
                terminal.Enable,
                terminal.TransferModelEnable,
                OpenClose = new
                {
                    terminal.ReactionCabin.IsFull,
                    terminal.ReactionCabin.IsEmpty,
                },
                Power = new
                {
                    ExhaustMain = terminal.ExhaustMain.PowerStatus,
                    ExhaustSlave = terminal.ExhaustSlave.PowerStatus,
                    GrayFan = terminal.GrayFan.PowerStatus,
                    PrimaryPlasmaGenerator = terminal.PlasmaGeneratorGroup.Primary.PowerStatus,
                    SecondaryPlasmaGenerator = terminal.PlasmaGeneratorGroup.Second.PowerStatus,
                    Pulverizer = terminal.Pulverizer.PowerStatus,
                    PrimaryPump = terminal.PrimaryPump.PowerStatus,
                    SecondaryPump = terminal.SecondaryPump.PowerStatus,
                    Transfer = terminal.Transfer.PowerStatus,
                    UVLight = terminal.UVLight.PowerStatus,
                }
            };
            var hubContext = context.GetService<IHubContext<GarbageTerminalHub>>();
            hubContext.Clients.All.SendAsync("status", status);
        }
    }
}