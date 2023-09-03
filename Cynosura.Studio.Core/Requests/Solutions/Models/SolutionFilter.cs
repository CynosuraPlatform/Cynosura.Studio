using System;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Solutions.Models
{
    public class SolutionFilter : EntityFilter
    {
        public string? Name { get; set; }
        public string? Path { get; set; }
    }
}
