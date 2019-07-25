using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Studio.Generator.Models
{
    public class SolutionMetadata
    {
        public string Name { get; set; }
        public string TemplateVersion { get; set; }
        public string TemplateName { get; set; } = "Cynosura.Template";
    }
}
