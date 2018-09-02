namespace Cynosura.Studio.Core.Generator.Models
{
    public class CodeTemplate
    {
        public string FilePath { get; set; }
        public string FileName { get; set; }
        public string TemplatePath { get; set; }
        public TemplateType Type { get; set; }
        public string InsertAfter { get; set; }
    }

    public enum TemplateType
    {
        Entity,
        View,
    }
}
