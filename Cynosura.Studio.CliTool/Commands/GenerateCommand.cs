using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Cynosura.Studio.Generator;
using Cynosura.Studio.Generator.Models;

namespace Cynosura.Studio.CliTool.Commands
{
    public class GenerateCommand: AppCommand
    {
        private readonly Dictionary<string, Func<IEnumerable<string>, Task<bool>>> _actions;

        public GenerateCommand(string solutionDirectory, string feed, string src, string templateName, ILifetimeScope lifetimeScope)
            : base(solutionDirectory, feed, src, templateName, lifetimeScope)
        {
            _actions = new Dictionary<string, Func<IEnumerable<string>, Task<bool>>>
            {
                {"entity", GenerateEntityActionAsync},
                {"enum", GenerateEnumActionAsync},
                {"all", GenerateAllActionAsync},
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
                return false;
            }
        }

        private async Task<bool> GenerateAllActionAsync(IEnumerable<string> arg)
        {
            var accessor = new SolutionAccessor(SolutionDirectory);
            var entities = await accessor.GetEntitiesAsync();
            foreach (var entity in entities.Where(w => !w.IsAbstract))
            {
                await GenerateEntityAsync(entity.Name);
                Console.WriteLine($"Entity {entity.Name} generated successfully");
            }
            var enums = await accessor.GetEnumsAsync();
            foreach (var en in enums)
            {
                await GenerateEnumAsync(en.Name);
                Console.WriteLine($"Enum {en.Name} generated successfully");
            }

            return true;
        }

        private async Task<bool> GenerateEnumActionAsync(IEnumerable<string> args)
        {
            var ar = args as string[] ?? args.ToArray();
            if (ar.Length != 1)
            {
                Console.WriteLine($"Command syntax: {CliApp.CommandName} generate enum <enumName>");
                return false;
            }
            var name = ar.FirstOrDefault();
            await GenerateEnumAsync(name);
            Console.WriteLine($"Enum {name} generated successfully");
            return true;
        }

        private async Task GenerateEnumAsync(string name)
        {
            var accessor = new SolutionAccessor(SolutionDirectory);
            var enums = await accessor.GetEnumsAsync();
            var generator = LifetimeScope.Resolve<CodeGenerator>();
            var en = enums.FirstOrDefault(f => f.Name == name);
            if (en == null)
            {
                throw new Exception($"Enum {name} not found");
            }
            await generator.GenerateEnumAsync(accessor, en);
            await generator.GenerateEnumViewAsync(accessor, new View(), en);
        }

        private async Task<bool> GenerateEntityActionAsync(IEnumerable<string> args)
        {
            var ar = args as string[] ?? args.ToArray();
            if (ar.Length != 1)
            {
                Console.WriteLine($"Command syntax: {CliApp.CommandName} generate entity <entityName>");
                return false;
            }
            var name = ar.FirstOrDefault();
            await GenerateEntityAsync(name);
            Console.WriteLine($"Entity {name} generated successfully");
            return true;
        }

        private async Task GenerateEntityAsync(string name)
        {
            var accessor = new SolutionAccessor(SolutionDirectory);
            var entities = await accessor.GetEntitiesAsync();
            var generator = LifetimeScope.Resolve<CodeGenerator>();
            var entity = entities.FirstOrDefault(f => f.Name == name);
            if (entity == null)
            {
                throw new Exception($"Entity {name} not found");
            }
            await generator.GenerateEntityAsync(accessor, entity);
            await generator.GenerateViewAsync(accessor, new View(), entity);
        }

        public override string Help()
        {
            return $"{CliApp.CommandName} generate <action>\r\n" +
                   $"Available actions: \r\n{string.Join("\r\n\t", _actions.Keys)}\r\n";
        }
    }
}
