using System;
using System.Collections.Generic;

namespace Cynosura.Studio.CliTool
{
    public interface IConfigService
    {
        /// <summary>
        /// Format "key1=value1"
        /// "key1=value1,key1.sub1=someValue"
        /// </summary>
        /// <param name="expression"></param>
        Dictionary<string, string> OverrideSettingsValue(string expression);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns>Item1 - commands, Items2 - properties</returns>
        Tuple<string[], IEnumerable<KeyValuePair<string, string>>> PrepareProperties(string[] args);
    }
}
