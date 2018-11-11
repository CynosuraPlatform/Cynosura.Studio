using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Studio.Core.Generator.Models;

namespace Cynosura.Studio.Core.Generator
{
    public class EnumViewModel
    {
        public View View { get; set; }
        public Models.Enum Enum { get; set; }
        public SolutionAccessor Solution { get; set; }
    }
}
