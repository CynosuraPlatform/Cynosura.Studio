using System.Collections.Generic;
using Cynosura.Studio.Generator.Models;

namespace Cynosura.Studio.Generator
{
    public class GenerateInfo
    {
        public required IGenerationObject GenerationObject { get; set; }
        public required object Model { get; set; }
        public required IEnumerable<TemplateType> Types { get; set; }
        public View? View { get; set; }
    }
}
