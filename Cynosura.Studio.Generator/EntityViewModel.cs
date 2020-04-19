using Cynosura.Studio.Generator.Models;

namespace Cynosura.Studio.Generator
{
    public class EntityViewModel
    {
        public EntityViewModel()
        {
        }

        public EntityViewModel(View view, Entity entity, SolutionAccessor solution)
        {
            View = view;
            Entity = entity;
            Solution = solution;
        }

        public View View { get; set; }
        public Entity Entity { get; set; }
        public SolutionAccessor Solution { get; set; }

        public GenerateInfo GetGenerateInfo()
        {
            return new GenerateInfo
            {
                GenerationObject = Entity,
                Model = this,
                Types = Entity.GetTemplateTypes(),
                View = View,
            };
        }
    }
}
