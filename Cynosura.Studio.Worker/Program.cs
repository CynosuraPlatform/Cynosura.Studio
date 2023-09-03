using System;
using System.Configuration;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Cynosura.Studio.Core;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Data;
using Cynosura.Studio.Infrastructure;
using Cynosura.Studio.Worker.Infrastructure;

namespace Cynosura.Studio.Worker
{
    class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureLogging((hostingContext, logging) =>
                {
                    logging.AddSimpleConsole(c =>
                    {
                        c.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
                    });
                })
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddDbContext<DataContext>(options =>
                    {
                        options.UseSqlite(hostContext.Configuration.GetConnectionString("DefaultConnection"));
                    });

                    services.AddIdentityCore<User>()
                        .AddRoles<Role>();

                    services.AddTransient(typeof(IStringLocalizer<>), typeof(DummyLocalizer<>));

                    services.AddOptions();

                    services.AddWorker(hostContext.Configuration);
                    services.AddInfrastructure(hostContext.Configuration);
                    services.AddData();
                    services.AddCore(hostContext.Configuration);
                });
        }
    }
}

