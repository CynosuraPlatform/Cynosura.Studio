using System;
using System.Collections.Generic;
using System.Text;

namespace Cynosura.Studio.Generator
{
    public class EnumModel
    {
        public EnumModel()
        {
        }

        public EnumModel(Models.Enum @enum, SolutionAccessor solution)
        {
            Enum = @enum;
            Solution = solution;
        }

        public Models.Enum Enum { get; set; }
        public SolutionAccessor Solution { get; set; }

        public GenerateInfo GetGenerateInfo()
        {
            return new GenerateInfo
            {
                GenerationObject = Enum,
                Model = this,
                Types = Enum.GetTemplateTypes(),
            };
        }
    }
}
