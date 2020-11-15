using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Cynosura.Studio.Data;

namespace Cynosura.Studio.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var databaseInitializer = (IDatabaseInitializer)services.GetService(typeof(IDatabaseInitializer));
                databaseInitializer.SeedAsync().GetAwaiter().GetResult();
            }
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>()
                        .ConfigureAppConfiguration((hostingContext, config) =>
                        {
                            var useProfileSettings =
                                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                                    Path.Combine(".cynosura", "appsettings.json"));
                            config.SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
                                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                                .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json",
                                    optional: true, reloadOnChange: true)
                                .AddJsonFile(
                                    useProfileSettings, optional: true)
                                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
                            config.AddEnvironmentVariables();
                        })
                        .ConfigureLogging(logging =>
                        {
                            logging.AddConsole(c =>
                            {
                                c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
                            });
                        });
                });
    }
}
