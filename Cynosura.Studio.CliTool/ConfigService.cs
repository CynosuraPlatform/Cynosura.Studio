using System;
using System.Collections.Generic;

namespace Cynosura.Studio.CliTool
{
    public class ConfigService : IConfigService
    {
        /// <summary>
        /// Format "key1=value1"
        /// "key1=value1,key1.sub1=someValue"
        /// </summary>
        /// <param name="expression"></param>
        public Dictionary<string, string> OverrideSettingsValue(string expression)
        {
            if (!expression.Contains('='))
                throw new Exception($"Invalid format in '{expression}'");
            var settingsOverrides = new Dictionary<string, string>();
            var key = "";
            var value = "";
            var text = false;
            var i = 0;
            var state = 0;
            while (i < expression.Length)
            {
                var last = i > 0 ? expression[i - 1] : (char?)null;
                var c = expression[i];
                i++;

                if (c == '"' && (last == null || last != '\\'))
                {
                    text = !text;
                }
                if (c == '=' && state == 0 && !text)
                {
                    state = 1;
                    continue;
                }
                if (c == ',' && state == 1 && !text)
                {
                    state = 0;
                    settingsOverrides.Add(key, value);
                    key = "";
                    value = "";
                    text = false;
                    continue;
                }
                if (state == 0)
                {
                    if (c == '.')
                        key += ':';
                    else key += c;
                }
                if (state == 1)
                {
                    value += c;
                }
            }
            if (state == 1)
                settingsOverrides.Add(key, value);
            return settingsOverrides;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Item1 - commands, Items2 - properties</returns>
        public Tuple<string[], IEnumerable<KeyValuePair<string, string>>> PrepareProperties(string[] args)
        {
            var commandArguments = new List<string>();
            var properties = new List<KeyValuePair<string, string>>();
            var skipIndex = -1;
            for (var i = 0; i < args.Length; i++)
            {
                var part = args[i];
                if (part.StartsWith("--"))
                {
                    skipIndex = i + 1;
                    var prop = part.Substring(2);
                    var value = args.Length < i ? "" : args[i + 1];
                    properties.Add(new KeyValuePair<string, string>(prop, value));
                    continue;
                }

                if (skipIndex != i)
                {
                    commandArguments.Add(part);
                }
            }

            return new Tuple<string[], IEnumerable<KeyValuePair<string, string>>>(commandArguments.ToArray(), properties);
        }

    }
}
