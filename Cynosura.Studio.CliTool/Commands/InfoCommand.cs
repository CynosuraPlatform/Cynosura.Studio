using System;
using System.Threading.Tasks;
using Autofac;
using Cynosura.Studio.Generator;

namespace Cynosura.Studio.CliTool.Commands
{
    public class InfoCommand: AppCommand
    {
        public InfoCommand(string solutionDirectory, string feed, string src, string templateName, ILifetimeScope lifetimeScope)
            : base(solutionDirectory, feed, src, templateName, lifetimeScope)
        {
        }

        public override Task<bool> ExecuteAsync(string[] args)
        {
            var accessor = new SolutionAccessor(SolutionDirectory);
            var meta = accessor.Metadata;
            Console.WriteLine(
                $"Solution name: {meta.Name}\r\nTemplate name:{meta.TemplateName}\r\nTemplate version: {meta.TemplateVersion}");
            return Task.FromResult(true);
        }

        public override string Help()
        {
            return $"Command usage: {CliApp.CommandName} info";
        }
    }
}
