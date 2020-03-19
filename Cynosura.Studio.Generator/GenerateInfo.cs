using System.Collections.Generic;
using Cynosura.Studio.Generator.Models;

namespace Cynosura.Studio.Generator
{
    public class GenerateInfo
    {
        public IGenerationObject GenerationObject { get; set; }
        public object Model { get; set; }
        public IEnumerable<TemplateType> Types { get; set; }
    }
}
