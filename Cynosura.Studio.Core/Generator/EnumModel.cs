using System;
using System.Collections.Generic;
using System.Text;
using Enum = Cynosura.Studio.Core.Generator.Models.Enum;

namespace Cynosura.Studio.Core.Generator
{
    public class EnumModel
    {
        public EnumModel()
        {
        }

        public EnumModel(Enum @enum, SolutionAccessor solution)
        {
            Enum = @enum;
            Solution = solution;
        }

        public Models.Enum Enum { get; set; }
        public SolutionAccessor Solution { get; set; }
    }
}
