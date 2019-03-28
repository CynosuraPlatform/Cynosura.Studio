using System;
using System.Configuration;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Data;
using Cynosura.Studio.Worker.Infrastructure;

namespace Cynosura.Studio.Worker
{
    class Program
    {
        public static async Task Main(string[] args)
        {
            NLog.LogManager.LoadConfiguration("nlog.config");
            var builder = new HostBuilder()
                .ConfigureHostConfiguration(configHost =>
                {
                    configHost.AddJsonFile("hostsettings.json", optional: true);
                    configHost.AddEnvironmentVariables();
                })
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                    config.AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                        optional: true);
                    config.AddJsonFile("appsettings.local.json", optional: true);
                    config.AddEnvironmentVariables();

                    if (args != null)
                    {
                        config.AddCommandLine(args);
                    }
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<DataContext>(options =>
                    {
                        options.UseSqlServer(hostContext.Configuration.GetConnectionString("DefaultConnection"));
                    });
                    services.AddIdentity<User, Role>()
                        .AddEntityFrameworkStores<DataContext>()
                        .AddDefaultTokenProviders();

                    services.AddOptions();
                })
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureContainer<ContainerBuilder>((hostContext, container) =>
                {
                    AutofacConfig.ConfigureAutofac(container, hostContext.Configuration);
                })
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                    logging.AddNLog();
                });

            await builder.RunConsoleAsync();

            NLog.LogManager.Shutdown();
        }
    }
}