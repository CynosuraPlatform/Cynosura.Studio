using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using ConsoleTables;
using Cynosura.Studio.Generator;

namespace Cynosura.Studio.CliTool.Commands
{
    public class ListCommand: AppCommand
    {
        private readonly Dictionary<string, Func<IEnumerable<string>, Task<bool>>> _actions;
        public ListCommand(string solutionDirectory, string feed, string src, string templateName, ILifetimeScope lifetimeScope)
            : base(solutionDirectory, feed, src, templateName, lifetimeScope)
        {
            _actions = new Dictionary<string, Func<IEnumerable<string>, Task<bool>>>
            {
                {"entity", ListEntitiesActionAsync},
                {"entities", ListEntitiesActionAsync},
                {"enum", ListEnumsActionAsync},
                {"enums", ListEnumsActionAsync},
                {
                    "help", (_) =>
                    {
                        Console.Write(Help());
                        return Task.FromResult(true);
                    }
                }
            };
        }

        public override async Task<bool> ExecuteAsync(string[] args)
        {
            
            var command = args.FirstOrDefault();
            if (!string.IsNullOrEmpty(command) && _actions.ContainsKey(command))
            {
                return await _actions[command].Invoke(GetArguments(args.Skip(1)));
            }
            else
            {
                Console.Write(Help());
            }

            return false;
        }

        private async Task<bool> ListEntitiesActionAsync(IEnumerable<string> args)
        {
            var accessor = new SolutionAccessor(SolutionDirectory);
            var entities = await accessor.GetEntitiesAsync();
            var table = new ConsoleTable("Name", "DisplayName");
            foreach (var entity in entities)
            {
                table.AddRow(entity.Name, entity.DisplayName);
            }
            table.Write();
            return true;
        }

        private async Task<bool> ListEnumsActionAsync(IEnumerable<string> args)
        {
            var accessor = new SolutionAccessor(SolutionDirectory);
            var enums = await accessor.GetEnumsAsync();
            var table = new ConsoleTable("Name", "DisplayName");
            foreach (var en in enums)
            {
                table.AddRow(en.Name, en.DisplayName);
            }
            table.Write();
            return true;
        }

        public override string Help()
        {
            return $"{CliApp.CommandName} list <action>\r\n" +
                   $"Available actions: \r\n{string.Join("\r\n", _actions.Keys.Select(s => $"\t{s}"))}\r\n";
        }
    }
}
