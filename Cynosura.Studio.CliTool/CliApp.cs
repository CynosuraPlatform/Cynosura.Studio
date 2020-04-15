using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Cynosura.Studio.CliTool.Commands;
using Cynosura.Studio.Generator;
using Cynosura.Studio.Generator.PackageFeed;
using Cynosura.Studio.Generator.PackageFeed.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Studio.CliTool
{
    public class CliApp
    {
        private readonly IConfigurationRoot _configurationRoot;
        private IConfigService _configService;
        private string _solutionDirectory;
        private string _feed;
        private string _src;
        private string _templateName = "Cynosura.Template";

        private string[] _arguments;
        private Dictionary<string, string> _settingsOverrides;
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
                {"src", value => _src = value },
                {"templateName", value=> _templateName = value },
                {"set", OverrideSettingsValue }
            };
            _configService = new ConfigService();
            _settingsOverrides = new Dictionary<string, string>();
            _solutionDirectory = Directory.GetCurrentDirectory();
            var (arguments, props) = _configService.PrepareProperties(args);
            _arguments = arguments;
            foreach (var (key, value) in props)
            {
                if (_setProps.ContainsKey(key))
                {
                    _setProps[key](value);
                }
                else
                {
                    throw new Exception($"Invalid prop {key}");
                }
            }

            var defaultConfig = new Dictionary<string, string>();

            foreach (var (key, value) in _settingsOverrides)
            {
                if (!defaultConfig.ContainsKey(key))
                    defaultConfig.Add(key, value);
            }

            if (!defaultConfig.ContainsKey("Nuget:FeedUrl"))
                defaultConfig.Add("Nuget:FeedUrl", _feed ?? "https://api.nuget.org/v3/index.json");
            if (!defaultConfig.ContainsKey("LocalFeed:SourcePath"))
                defaultConfig.Add("LocalFeed:SourcePath", _src);

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddInMemoryCollection(defaultConfig);

            _configurationRoot = builder.Build();

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<NugetSettings>(_configurationRoot.GetSection("Nuget"));
            services.Configure<LocalFeedOptions>(_configurationRoot.GetSection("LocalFeed"));
            services.AddLogging();

            services.AddCliTool();
            services.AddGenerator(_configurationRoot);
        }

        public void Configure(IServiceProvider serviceProvider)
        {
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

        private void OverrideSettingsValue(string expression)
        {
            var overrides = _configService.OverrideSettingsValue(expression);
            foreach (var (key, value) in overrides)
            {
                if (!_settingsOverrides.ContainsKey(key))
                {
                    _settingsOverrides.Add(key, value);
                }
            }
        }


        public async Task<bool> StartAsync(ServiceProvider serviceProvider)
        {
            
            Console.CancelKeyPress += (sender, e) => e.Cancel = true;
            var commands = new Dictionary<string, AppCommand>
            {
                {"list", new ListCommand(_solutionDirectory, _feed, _src,_templateName, serviceProvider) },
                {"generate", new GenerateCommand(_solutionDirectory, _feed, _src,_templateName, serviceProvider) },
                {"new", new NewCommand(_solutionDirectory, _feed, _src,_templateName, serviceProvider) },
                {"upgrade", new UpgradeCommand(_solutionDirectory, _feed, _src,_templateName, serviceProvider) },
                {"info", new InfoCommand(_solutionDirectory, _feed, _src,_templateName, serviceProvider) }
            };
            var helpProps = _setProps.Keys.ToDictionary(k => k, v => v);
            helpProps["debug"] = "yes";
            helpProps["src"] = "local feed path";
            commands.Add("help",
                new HelpCommand(_solutionDirectory, _feed, _src, _templateName, serviceProvider, commands, helpProps));
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
