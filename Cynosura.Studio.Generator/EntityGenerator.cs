using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Studio.Generator.Models;

namespace Cynosura.Studio.Generator
{
    public class EntityGenerator
    {
        private readonly CodeGenerator _codeGenerator;

        public EntityGenerator(CodeGenerator codeGenerator)
        {
            _codeGenerator = codeGenerator;
        }

        public async Task GenerateEntityAsync(SolutionAccessor solution, Entity entity)
        {
            var model = new EntityModel(entity, solution);
            await _codeGenerator.GenerateAsync(solution, model.GetGenerateInfo());
        }

        public async Task UpgradeEntityAsync(SolutionAccessor solution, Entity oldEntity, Entity newEntity)
        {
            await UpgradeEntitiesAsync(solution, new[] { oldEntity }, new[] { newEntity });
        }

        public async Task UpgradeEntitiesAsync(SolutionAccessor solution, IEnumerable<Entity> oldEntities, IEnumerable<Entity> newEntities)
        {
            var oldGenerateInfos = oldEntities
                .Select(oldEntity => new EntityModel(oldEntity, solution))
                .Select(model => model.GetGenerateInfo());
            var newGenerateInfos = newEntities
                .Select(newEntity => new EntityModel(newEntity, solution))
                .Select(model => model.GetGenerateInfo());

            await _codeGenerator.UpgradeAsync(solution, oldGenerateInfos, newGenerateInfos);
        }

        public async Task GenerateEntityViewAsync(SolutionAccessor solution, Entity entity)
        {
            var views = await solution.GetViewsAsync();
            foreach (var view in views)
            {
                var model = new EntityViewModel(view, entity, solution);
                await _codeGenerator.GenerateAsync(solution, model.GetGenerateInfo());
            }
        }

        public async Task UpgradeEntityViewAsync(SolutionAccessor solution, Entity oldEntity, Entity newEntity)
        {
            await UpgradeEntityViewsAsync(solution, new[] { oldEntity }, new[] { newEntity });
        }

        public async Task UpgradeEntityViewsAsync(SolutionAccessor solution, IEnumerable<Entity> oldEntities, IEnumerable<Entity> newEntities)
        {
            var views = await solution.GetViewsAsync();
            var oldGenerateInfos = oldEntities
                .SelectMany(oldEntity => views.Select(v => new EntityViewModel(v, oldEntity, solution)))
                .Select(model => model.GetGenerateInfo());
            var newGenerateInfos = newEntities
                .SelectMany(newEntity => views.Select(v => new EntityViewModel(v, newEntity, solution)))
                .Select(model => model.GetGenerateInfo());

            await _codeGenerator.UpgradeAsync(solution, oldGenerateInfos, newGenerateInfos);
        }

        internal async Task CopyEntitiesAsync(SolutionAccessor fromSolution, SolutionAccessor toSolution)
        {
            var fromEntities = await fromSolution.GetEntitiesAsync();
            fromEntities = SortByDependency(fromEntities).ToList();
            var toEntities = await toSolution.GetEntitiesAsync();
            var oldEntitiesToUpgrade = new List<Entity>();
            var newEntitiesToUpgrade = new List<Entity>();
            foreach (var entity in fromEntities)
            {
                var toEntity = toEntities.FirstOrDefault(e => e.Id == entity.Id);
                if (toEntity == null)
                {
                    await toSolution.CreateEntityAsync(entity);
                    var newEntity = (await toSolution.GetEntitiesAsync())
                        .FirstOrDefault(e => e.Id == entity.Id);
                    await GenerateEntityAsync(toSolution, newEntity);
                    await GenerateEntityViewAsync(toSolution, newEntity);
                }
                else
                {
                    await toSolution.UpdateEntityAsync(entity);
                    var newEntity = (await toSolution.GetEntitiesAsync())
                        .FirstOrDefault(e => e.Id == entity.Id);
                    oldEntitiesToUpgrade.Add(toEntity);
                    newEntitiesToUpgrade.Add(newEntity);
                }
            }

            if (oldEntitiesToUpgrade.Count > 0)
            {
                var oldGenerateInfos = oldEntitiesToUpgrade
                    .SelectMany(oldEntity => new[] {
                        new EntityModel(oldEntity, toSolution).GetGenerateInfo(),
                        new EntityViewModel(new View(), oldEntity, toSolution).GetGenerateInfo()
                    });
                var newGenerateInfos = newEntitiesToUpgrade
                    .SelectMany(newEntity => new[] {
                        new EntityModel(newEntity, toSolution).GetGenerateInfo(),
                        new EntityViewModel(new View(), newEntity, toSolution).GetGenerateInfo()
                    });
                await _codeGenerator.UpgradeAsync(toSolution, oldGenerateInfos, newGenerateInfos);
            }
        }

        private IEnumerable<Entity> SortByDependency(IEnumerable<Entity> entities)
        {
            var entityList = entities.ToList();
            var sortedList = new List<Entity>();
            while (entityList.Count > 0)
            {
                var okEntities = entityList
                    .Where(e => e.DependentEntities.Count == 0 ||
                                e.DependentEntities.All(de => sortedList.Contains(de)))
                    .ToList();
                if (okEntities.Count == 0)
                    throw new Exception("Cannot sort by dependency");
                foreach (var okEntity in okEntities)
                {
                    entityList.Remove(okEntity);
                    sortedList.Add(okEntity);
                }
            }

            return sortedList;
        }
    }
}
