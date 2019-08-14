using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Studio.CliTool
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var app = new CliApp(args);
            var services = new ServiceCollection();
            var serviceProvider = app.ConfigureServices(services);
            app.Configure(serviceProvider);
            try
            {
                var result = await app.StartAsync();
                if (!result)
                {
                    Environment.ExitCode = 1;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.GetType().Name}: {e.Message}");
                Environment.ExitCode = 1;
            }
        }
    }
}
