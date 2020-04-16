using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Studio.Generator.Models
{
    public class SolutionMetadata
    {
        public string Name { get; set; }
        [Obsolete]
        public string Version
        {
            get => TemplateVersion;
            set => TemplateVersion = value;
        }
        public string TemplateVersion { get; set; }
        public string TemplateName { get; set; } = "Cynosura.Template";

        public bool ShouldSerializeVersion()
        {
            return false;
        }
        
        public string SolutionFile { get; set; }
    }
}
