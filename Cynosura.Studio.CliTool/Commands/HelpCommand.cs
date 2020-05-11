﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Studio.CliTool.Commands
{
    public class HelpCommand: AppCommand
    {
        private readonly IDictionary<string, AppCommand> _commands;
        private readonly IEnumerable<KeyValuePair<string, string>> _props;

        public HelpCommand(string solutionDirectory, string feed, string src, string templateName, ServiceProvider serviceProvider, IDictionary<string, AppCommand> commands, IEnumerable<KeyValuePair<string, string>> props) 
            : base(solutionDirectory, feed, src, templateName, serviceProvider)
        {
            _commands = commands;
            _props = props;
        }

        public override Task<bool> ExecuteAsync(string[] args)
        {
            var name = args.FirstOrDefault();
            if (string.IsNullOrEmpty(name))
            {
                var commands = string.Join("\r\n", _commands.Keys.Select(s => $"\t{s}"));
                var props = string.Join(" ", _props.Select(s => $"[{s.Key} prop-value]"));
                Console.WriteLine("cyn <command> [command arguments] {0}\r\n\r\nAvailable commands: \r\n{1}",
                    props,
                    commands);
                return Task.FromResult(true);
            }
            if (!_commands.ContainsKey(name))
            {
                Console.WriteLine($"Command {name} not found\r\n {Help()}");
                return Task.FromResult(false);
            }
            var helpString = _commands[name].Help();
            Console.WriteLine(helpString);
            return Task.FromResult(true);
        }

        public override string Help()
        {
            return $"Command usage:\r\n\t{CliApp.CommandName} help\t\tView all commands\r\n\t{CliApp.CommandName} help <commandName>\tView <commandName> information";
        }
    }
}
