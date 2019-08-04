using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;

namespace Cynosura.Studio.CliTool.Commands
{
    public class HelpCommand: AppCommand
    {
        private readonly IDictionary<string, AppCommand> _commands;

        public HelpCommand(string solutionDirectory, string feed, string src, string templateName, ILifetimeScope lifetimeScope, IDictionary<string, AppCommand> commands) : base(solutionDirectory, feed, src, templateName, lifetimeScope)
        {
            _commands = commands;
        }

        public override Task<bool> ExecuteAsync(string[] args)
        {
            var name = args.FirstOrDefault();
            if (string.IsNullOrEmpty(name))
            {
                var commands = _commands.Aggregate("", (a, b) => a + $"\r\n\r\n\t{b.Key}\r\n{b.Value.Help()}");
                Console.WriteLine($"Available commands: {commands}");
                return Task.FromResult(true);
            }
            if (_commands.ContainsKey(name))
            {
                Console.WriteLine($"Command not found\r\n {Help()}");
                return Task.FromResult(false);
            }
            var helpString = _commands[name].Help();
            Console.WriteLine(helpString);
            return Task.FromResult(true);
        }

        public override string Help()
        {
            return $"Command usage:\r\n\t{CliApp.CommandName} help\tView all commands\r\n\t{CliApp.CommandName} help <commandName>\tView <commandName> information";
        }
    }
}
