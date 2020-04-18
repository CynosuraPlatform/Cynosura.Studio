﻿using Cynosura.Studio.Generator.Models;

namespace Cynosura.Studio.Generator
{
    public class EntityModel
    {
        public EntityModel()
        {
        }

        public EntityModel(Entity entity, SolutionAccessor solution)
        {
            Entity = entity;
            Solution = solution;
        }

        public Entity Entity { get; set; }
        public SolutionAccessor Solution { get; set; }

        public GenerateInfo GetGenerateInfo()
        {
            return new GenerateInfo
            {
                GenerationObject = Entity,
                Model = this,
                Types = Entity.GetTemplateTypes(),
            };
        }
    }
}
