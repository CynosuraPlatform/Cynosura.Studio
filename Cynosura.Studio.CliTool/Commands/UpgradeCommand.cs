using System;
using System.Threading.Tasks;
using Cynosura.Studio.Generator;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Studio.CliTool.Commands
{
    public class UpgradeCommand: AppCommand
    {
        public UpgradeCommand(string solutionDirectory, string feed, string src, string templateName, ServiceProvider serviceProvider) 
            : base(solutionDirectory, feed, src, templateName, serviceProvider)
        {
        }

        public override async Task<bool> ExecuteAsync(string[] args)
        {
            var generator = ServiceProvider.GetService<SolutionGenerator>();
            var accessor = new SolutionAccessor(SolutionDirectory);
            await generator.UpgradeSolutionAsync(accessor);
            Console.WriteLine($"Solution {accessor.Namespace} upgraded successfully");
            return true;
        }

        public override string Help()
        {
            return $"Command syntax: {CliApp.CommandName} upgrade";
        }
    }
}
