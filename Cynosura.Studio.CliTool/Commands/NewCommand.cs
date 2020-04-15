using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cynosura.Studio.Generator;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Studio.CliTool.Commands
{
    public class NewCommand: AppCommand
    {
        public NewCommand(string solutionDirectory, string feed, string src, string templateName, ServiceProvider serviceProvider) 
            : base(solutionDirectory, feed, src, templateName, serviceProvider)
        {
        }

        public override async Task<bool> ExecuteAsync(string[] args)
        {
            var name = args.FirstOrDefault();
            var templateName = args.Skip(1).FirstOrDefault() ?? TemplateName;
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(templateName))
            {
                Console.WriteLine(Help());
                return false;
            }
            var generator = ServiceProvider.GetService<CodeGenerator>();
            if (!Directory.Exists(SolutionDirectory))
            {
                Directory.CreateDirectory(SolutionDirectory);
            }
            await generator.GenerateSolutionAsync(SolutionDirectory, name, templateName);
            Console.WriteLine($"Solution {name} created on path {SolutionDirectory}");
            return true;
        }

        public override string Help()
        {
            return $"Command syntax: {CliApp.CommandName} new <name> --templateName <templateName>";
        }
    }
}
