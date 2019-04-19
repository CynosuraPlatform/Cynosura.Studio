using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cynosura.Studio.Core.Generator.Models
{
    public class CodeTemplate
    {
        public CodeTemplate()
        {
            Targets = new string[0];
        }
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string TemplatePath { get; set; }
        [Obsolete]
        public TemplateType Type {
            get { return Types.FirstOrDefault(); }
            set { Types = new[] { value }; }
        }
        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public IEnumerable<TemplateType> Types { get; set; }
        public string InsertAfter { get; set; }
        public IEnumerable<string> Targets { get; set; }

        public bool ShouldSerializeType()
        {
            return false;
        }
    }

    public enum TemplateType
    {
        Entity,
        View,
        Enum,
        EnumView,
    }
}
