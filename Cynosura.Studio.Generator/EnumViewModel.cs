using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Studio.Generator.Models;

namespace Cynosura.Studio.Generator
{
    public class EnumViewModel
    {
        public EnumViewModel()
        {
        }

        public EnumViewModel(View view, Models.Enum @enum, SolutionAccessor solution)
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
