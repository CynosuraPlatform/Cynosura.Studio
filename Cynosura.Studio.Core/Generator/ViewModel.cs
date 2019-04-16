using Cynosura.Studio.Core.Generator.Models;

namespace Cynosura.Studio.Core.Generator
{
    public class ViewModel
    {
        public ViewModel()
        {
        }

        public ViewModel(View view, Entity entity, SolutionAccessor solution)
        {
            View = view;
            Entity = entity;
            Solution = solution;
        }

        public View View { get; set; }
        public Entity Entity { get; set; }
        public SolutionAccessor Solution { get; set; }
    }
}
