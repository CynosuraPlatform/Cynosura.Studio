using System.Collections.Generic;

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
        public TemplateType Type { get; set; }
        public string InsertAfter { get; set; }
        public IEnumerable<string> Targets { get; set; }
    }

    public enum TemplateType
    {
        Entity,
        View,
        Enum,
        EnumView,
    }
}
