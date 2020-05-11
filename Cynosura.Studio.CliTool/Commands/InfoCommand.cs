using System;
using System.Reflection;
using System.Threading.Tasks;
using Cynosura.Studio.Generator;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Studio.CliTool.Commands
{
    public class InfoCommand: AppCommand
    {
        public InfoCommand(string solutionDirectory, string feed, string src, string templateName, ServiceProvider serviceProvider)
            : base(solutionDirectory, feed, src, templateName, serviceProvider)
        {
        }

        public override Task<bool> ExecuteAsync(string[] args)
        {
            var assembly = typeof(Program).Assembly;
            var appVersion = assembly
                .GetCustomAttribute<AssemblyFileVersionAttribute>().Version;
            var accessor = new SolutionAccessor(SolutionDirectory);
            var meta = accessor.Metadata;
            Console.WriteLine(
                $"Solution name: {meta.Name}\r\nTemplate name: {meta.TemplateName}\r\nTemplate version: {meta.TemplateVersion}\r\n" +
                $"Cli-Application version: {appVersion}\r\nCli location: {assembly.Location}\r\n");
            return Task.FromResult(true);
        }

        public override string Help()
        {
            return $"Command usage: {CliApp.CommandName} info";
        }
    }
}
