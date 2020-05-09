using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Studio.CliTool
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var app = new CliApp(args);
            var services = new ServiceCollection();
            app.ConfigureServices(services);
            try
            {
                var serviceProvider = services.BuildServiceProvider();
                var result = await app.StartAsync(serviceProvider);
                if (!result)
                {
                    Environment.ExitCode = 1;
                }
            }
            catch (Exception exception)
            {
                var ex = exception;
                var message = "";
                while (ex !=null)
                {
                    message += ex.Message + "\r\n";
                    ex = ex.InnerException;
                }
                Console.WriteLine($"{exception.GetType().Name}: {message}");
                Console.WriteLine($"StackTrace: {exception.StackTrace}");
                Console.WriteLine("You can track issues on https://github.com/CynosuraPlatform/Cynosura.Studio/issues");
                Environment.ExitCode = 1;
            }
        }
    }
}
