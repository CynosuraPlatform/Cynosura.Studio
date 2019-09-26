using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cynosura.Studio.CliTool.Commands;
using Cynosura.Studio.Generator.PackageFeed;
using Cynosura.Studio.Generator.PackageFeed.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Studio.CliTool
{
    public class CliApp
    {
        private IContainer _container;
        private ILifetimeScope _lifetimeScope;
        private readonly IConfigurationRoot _configurationRoot;

        private string _solutionDirectory;
        private string _feed;
        private string _feedUsername;
        private string _feedPassword;
        private string _src;
        private string _templateName;

        private string[] _arguments;
        private Dictionary<string, Action<string>> _setProps;

        public const string CommandName = "cyn";

        public CliApp(string[] args)
        {
            _setProps = new Dictionary<string, Action<string>>
            {
                {"solutionDirectory", SetDirectory},
                {"solution", SetDirectory},
                {"debug", AttachDebugger },
                {"feed", value => _feed = value },
                {"feedUsername", value => _feedUsername = value },
                {"feedPassword", value => _feedPassword = value },
                {"src", value => _src = value },
                {"templateName", value=> _templateName = value }
            };
            _solutionDirectory = Directory.GetCurrentDirectory();
            _arguments = PrepareProperties(args);
            var defaultConfig = new Dictionary<string, string>
            {
                {"Nuget:FeedUrl", _feed ?? "https://api.nuget.org/v3/index.json"},
                {"Nuget:Username", _feedUsername},
                {"Nuget:Password", _feedPassword},
                {"LocalFeed:SourcePath", _src}
            };
            var useProfileSettings =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), ".cynosura.json");
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile(useProfileSettings, optional: true)
                .AddJsonFile($"appsettings.local.json", optional: true)
                .AddInMemoryCollection(defaultConfig);

            _configurationRoot = builder.Build();
 
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<NugetSettings>(_configurationRoot.GetSection("Nuget"));
            services.Configure<LocalFeedOptions>(_configurationRoot.GetSection("LocalFeed"));
            services.AddLogging();
            var containerBuilder = new ContainerBuilder();
            AutofacConfig.ConfigureAutofac(containerBuilder, _configurationRoot);
            containerBuilder.Populate(services);
            var container = containerBuilder.Build();
            _container = container;
            _lifetimeScope = container.BeginLifetimeScope();
            return new AutofacServiceProvider(container);
        }

        public void Configure(IServiceProvider serviceProvider)
        {
        }

        private string[] PrepareProperties(string[] args)
        {
            var commandArguments = new List<string>();
            var skipIndex = -1;
            for (var i = 0; i < args.Length; i++)
            {
                var part = args[i];
                if (part.StartsWith("--"))
                {
                    skipIndex = i + 1;
                    var prop = part.Substring(2);
                    if (_setProps.ContainsKey(prop))
                    {
                        var value = args.Length < i ? "" : args[i + 1];
                        _setProps[prop].Invoke(value);
                    }
                    else
                    {
                        throw new Exception($"Invalid prop {prop}");
                    }
                    continue;
                }

                if (skipIndex != i)
                {
                    commandArguments.Add(part);
                }
            }

            return commandArguments.ToArray();
        }

        private void SetDirectory(string value)
        {
            var dir = new DirectoryInfo(value);
            _solutionDirectory = dir.FullName;
        }


        private void AttachDebugger(string value)
        {
            if (value == "yes")
            {
                System.Diagnostics.Debugger.Launch();
            }
        }

        public async Task<bool> StartAsync()
        {
            Console.CancelKeyPress += (sender, e) => e.Cancel = true;
            var commands = new Dictionary<string, AppCommand>
            {
                {"list", new ListCommand(_solutionDirectory, _feed, _src,_templateName, _lifetimeScope) },
                {"generate", new GenerateCommand(_solutionDirectory, _feed, _src,_templateName, _lifetimeScope) },
                {"new", new NewCommand(_solutionDirectory, _feed, _src,_templateName, _lifetimeScope) },
                {"upgrade", new UpgradeCommand(_solutionDirectory, _feed, _src,_templateName, _lifetimeScope) },
                {"info", new InfoCommand(_solutionDirectory, _feed, _src,_templateName, _lifetimeScope) }
            };
            var helpProps = _setProps.Keys.ToDictionary(k => k, v => v);
            helpProps["debug"] = "yes";
            helpProps["src"] = "local feed path";
            commands.Add("help",
                new HelpCommand(_solutionDirectory, _feed, _src, _templateName, _lifetimeScope, commands, helpProps));
            var command = _arguments.Length > 0 ? _arguments[0] : null;
            if (!string.IsNullOrEmpty(command) && commands.ContainsKey(command))
            {
                return await commands[command].ExecuteAsync(_arguments.Skip(1).ToArray());
            }
            else
            {
                await commands["help"].ExecuteAsync(new string[0]);
            }
            return true;
        }
    }
}
