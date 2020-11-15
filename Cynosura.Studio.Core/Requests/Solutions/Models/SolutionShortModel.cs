using System;
using System.Collections.Generic;

namespace Cynosura.Studio.Core.Requests.Solutions.Models
{
    public class SolutionShortModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
