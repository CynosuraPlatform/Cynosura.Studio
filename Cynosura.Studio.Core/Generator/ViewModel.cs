using Cynosura.Studio.Core.Generator.Models;

namespace Cynosura.Studio.Core.Generator
{
    public class ViewModel
    {
        public View View { get; set; }
        public Entity Entity { get; set; }
        public SolutionAccessor Solution { get; set; }
    }
}
