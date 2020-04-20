using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Studio.Generator.Models;

namespace Cynosura.Studio.Generator
{
    public class ViewGenerator
    {
        private readonly EntityGenerator _entityGenerator;
        private readonly EnumGenerator _enumGenerator;

        public ViewGenerator(EntityGenerator entityGenerator,
            EnumGenerator enumGenerator)
        {
            _entityGenerator = entityGenerator;
            _enumGenerator = enumGenerator;
        }

        public async Task GenerateViewAsync(SolutionAccessor solution, View view)
        {
            // TODO: Create new view project (from Template package)

            var entities = await solution.GetEntitiesAsync();
            foreach (var entity in entities)
            {
                await _entityGenerator.GenerateEntityViewAsync(solution, entity, view);
            }

            var enums = await solution.GetEnumsAsync();
            foreach (var @enum in enums)
            {
                await _enumGenerator.GenerateEnumViewAsync(solution, @enum, view);
            }
        }

        internal async Task CopyViewsAsync(SolutionAccessor fromSolution, SolutionAccessor toSolution)
        {
            var fromViews = await fromSolution.GetViewsAsync();
            var toViews = await toSolution.GetViewsAsync();
            foreach (var entity in fromViews)
            {
                var toView = toViews.FirstOrDefault(e => e.Id == entity.Id);
                if (toView == null)
                {
                    await toSolution.CreateViewAsync(entity);
                }
                else
                {
                    await toSolution.UpdateViewAsync(entity);
                }
            }
        }
    }
}
