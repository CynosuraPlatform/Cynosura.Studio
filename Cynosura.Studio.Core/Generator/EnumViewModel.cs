using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Studio.Core.Generator.Models;
using Enum = Cynosura.Studio.Core.Generator.Models.Enum;

namespace Cynosura.Studio.Core.Generator
{
    public class EnumViewModel
    {
        public EnumViewModel()
        {
        }

        public EnumViewModel(View view, Enum @enum, SolutionAccessor solution)
        {
            View = view;
            Enum = @enum;
            Solution = solution;
        }

        public View View { get; set; }
        public Models.Enum Enum { get; set; }
        public SolutionAccessor Solution { get; set; }
    }
}
