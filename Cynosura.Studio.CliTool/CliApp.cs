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
using Microsoft.Extensions.Logging;

namespace Cynosura.Studio.CliTool
{
    public class CliApp
    {
        private readonly IConfigurationRoot _configurationRoot;
        private IConfigService _configService;
        private string _solutionDirectory;
        private string _feed = "https://api.nuget.org/v3/index.json";
        private string _src;
        private string _templateName = "Cynosura.Template";
        public LogLevel LogLevel = LogLevel.Error;

        private string[] _arguments;
        private Dictionary<string, string> _settingsOverrides;

        private Dictionary<string, Action<string>> _setProps;
        private Dictionary<string, Action> _argumentKey;
        private string _useProfileSettingsFile;

        public const string CommandName = "cyn";
        public CliApp(string[] args)
        {
            _setProps = new Dictionary<string, Action<string>>
            {
                {"--solutionDirectory", SetDirectory},
                {"--solution", SetDirectory},
                {"--log", SetLogLevel},
                {"--feed", value => _feed = value },
                {"--src", value => _src = value },
                {"--templateName", value=> _templateName = value },
                {"--set", OverrideSettingsValue }
            };
            _argumentKey = new Dictionary<string, Action>
            {
                {"--debug", () => LogLevel = LogLevel.Debug},
                {"-v", () => LogLevel = LogLevel.Warning},
                {"-vv", () => LogLevel = LogLevel.Information},
                {"-vvv", () => LogLevel = LogLevel.Debug},
                {"-vvvv", () => LogLevel = LogLevel.Trace},
            };
            _configService = new ConfigService();
            _settingsOverrides = new Dictionary<string, string>();
            _solutionDirectory = Directory.GetCurrentDirectory();
            _arguments = ParseArguments(args);

            var defaultConfig = new Dictionary<string, string>();

            foreach (var (key, value) in _settingsOverrides)
            {
                if (!defaultConfig.ContainsKey(key))
                    defaultConfig.Add(key, value);
            }

            if (!defaultConfig.ContainsKey("Nuget:FeedUrl"))
                defaultConfig.Add("Nuget:FeedUrl", _feed);
            if (!defaultConfig.ContainsKey("LocalFeed:SourcePath"))
                defaultConfig.Add("LocalFeed:SourcePath", _src);

            _useProfileSettingsFile =
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                    Path.Combine(".cynosura", "appsettings.json"));

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddInMemoryCollection(defaultConfig)
                .AddJsonFile(_useProfileSettingsFile, optional: true);

            _configurationRoot = builder.Build();
        }

        public string[] ParseArguments(string[] args)
        {
            var arguments = new List<string>();
            var i = 0;
            while(i < args.Length)
            {
                var part = args[i];
                if (part.StartsWith("-"))
                {
                    if (_setProps.ContainsKey(part))
                    {
                        var next = i + 1 < args.Length ? args[i + 1] : "";
                        _setProps[part](next);
                        i += 2;
                        continue;
                    }
                    if (_argumentKey.ContainsKey(part))
                    {
                        _argumentKey[part]();
                        i++;
                        continue;
                    }
                    throw new Exception($"Invalid prop {part}");
                }

                arguments.Add(part);
                i++;
            }

            return arguments.ToArray();
        }

        private void SetLogLevel(string value)
        {
            if (!Enum.TryParse(value, out LogLevel))
            {
                var levels = Enum.GetNames(typeof(LogLevel));
                throw new Exception($"{value} is not accepted log level\r\nApplicable values: {levels}");
            }
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddOptions();
            services.Configure<NugetSettings>(_configurationRoot.GetSection("Nuget"));
            services.Configure<LocalFeedOptions>(_configurationRoot.GetSection("LocalFeed"));
            services.AddLogging(builder => builder.AddConsole())
                .Configure<LoggerFilterOptions>(options => options.MinLevel = LogLevel);

            services.AddCliTool();
            services.AddGenerator(_configurationRoot);
        }

        private void SetDirectory(string value)
        {
            var dir = new DirectoryInfo(value);
            _solutionDirectory = dir.FullName;
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
            var logger = serviceProvider.GetService<ILogger<CliApp>>();
            logger.LogInformation($"Working directory: {_solutionDirectory}");
            logger.LogInformation($"Feed: {_feed}");
            logger.LogInformation($"Local feed source path: {_src}");
            logger.LogInformation($"Template: {_templateName}");

            if (File.Exists(_useProfileSettingsFile))
            {
                logger.LogInformation($"Used settings from: {_useProfileSettingsFile}");
            }
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
