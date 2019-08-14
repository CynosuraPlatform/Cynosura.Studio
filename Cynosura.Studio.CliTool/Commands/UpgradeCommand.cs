using System;
using System.Threading.Tasks;
using Autofac;
using Cynosura.Studio.Generator;

namespace Cynosura.Studio.CliTool.Commands
{
    public class UpgradeCommand: AppCommand
    {
        public UpgradeCommand(string solutionDirectory, string feed, string src, string templateName, ILifetimeScope lifetimeScope) 
            : base(solutionDirectory, feed, src, templateName, lifetimeScope)
        {
        }

        public override async Task<bool> ExecuteAsync(string[] args)
        {
            var generator = LifetimeScope.Resolve<CodeGenerator>();
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
