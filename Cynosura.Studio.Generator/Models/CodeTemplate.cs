using System;
using System.Collections.Generic;
using System.Linq;
using Cynosura.Studio.Generator.Infrastructure;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Cynosura.Studio.Generator.Models
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

        public bool CheckTargets(PropertyCollection properties)
        {
            if (Targets == null)
                return true;
            var targets = Targets.ToList();
            return targets.Count == 0 ||
                   targets.All(a => properties[a] is bool val && val);
        }

        public bool CheckTypes(IEnumerable<TemplateType> templateTypes)
        {
            if (templateTypes == null)
                return false;
            var templateTypeList = templateTypes.ToList();
            return Types.Any(t => templateTypeList.Contains(t));
        }
    }

    public enum TemplateType
    {
        Entity,
        View,
        Enum,
        EnumView,
        AbstractEntity,
    }
}
