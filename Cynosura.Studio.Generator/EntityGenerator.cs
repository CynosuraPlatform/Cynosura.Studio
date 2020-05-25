using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cynosura.Studio.Generator.Infrastructure;
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
                await GenerateEntityViewAsync(solution, entity, view);
            }
        }

        public async Task GenerateEntityViewAsync(SolutionAccessor solution, Entity entity, View view)
        {
            var model = new EntityViewModel(view, entity, solution);
            await _codeGenerator.GenerateAsync(solution, model.GetGenerateInfo());
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

        internal async Task CopyEntitiesAsync(SolutionAccessor fromSolution, SolutionAccessor toSolution, SolutionAccessor mergeToSolution)
        {
            var fromEntities = await fromSolution.GetEntitiesAsync();
            fromEntities = SortByDependency(fromEntities).ToList();
            var toEntities = await toSolution.GetEntitiesAsync();
            var mergeToEntities = await mergeToSolution.GetEntitiesAsync();
            var oldEntitiesToUpgrade = new List<Entity>();
            var newEntitiesToUpgrade = new List<Entity>();
            var oldMergeEntitiesToUpgrade = new List<Entity>();
            var newMergeEntitiesToUpgrade = new List<Entity>();
            foreach (var entity in fromEntities)
            {
                var toEntity = toEntities.FirstOrDefault(e => e.Id == entity.Id);
                var mergeToEntity = mergeToEntities.FirstOrDefault(e => e.Id == entity.Id);
                if (toEntity == null)
                {
                    await toSolution.CreateEntityAsync(entity);
                    var newEntity = (await toSolution.GetEntitiesAsync())
                        .FirstOrDefault(e => e.Id == entity.Id);
                    await GenerateEntityAsync(toSolution, newEntity);
                    await GenerateEntityViewAsync(toSolution, newEntity);

                    await mergeToSolution.CreateEntityAsync(entity);
                    var newMergeEntity = (await mergeToSolution.GetEntitiesAsync())
                        .FirstOrDefault(e => e.Id == entity.Id);
                    await GenerateEntityAsync(mergeToSolution, newMergeEntity);
                    await GenerateEntityViewAsync(mergeToSolution, newMergeEntity);
                }
                else
                {
                    await toSolution.UpdateEntityAsync(entity);
                    var newEntity = (await toSolution.GetEntitiesAsync())
                        .FirstOrDefault(e => e.Id == entity.Id);
                    oldEntitiesToUpgrade.Add(toEntity);
                    newEntitiesToUpgrade.Add(newEntity);

                    var newMergeEntity = mergeToEntity.SerializeToJson().DeserializeFromJson<Entity>();
                    MergeEntity(entity, toEntity, newMergeEntity);
                    await mergeToSolution.UpdateEntityAsync(newMergeEntity);
                    newMergeEntity = (await mergeToSolution.GetEntitiesAsync())
                        .FirstOrDefault(e => e.Id == entity.Id);
                    oldMergeEntitiesToUpgrade.Add(mergeToEntity);
                    newMergeEntitiesToUpgrade.Add(newMergeEntity);
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

            if (oldMergeEntitiesToUpgrade.Count > 0)
            {
                var oldGenerateInfos = oldMergeEntitiesToUpgrade
                    .SelectMany(oldEntity => new[] {
                        new EntityModel(oldEntity, mergeToSolution).GetGenerateInfo(),
                        new EntityViewModel(new View(), oldEntity, mergeToSolution).GetGenerateInfo()
                    });
                var newGenerateInfos = newMergeEntitiesToUpgrade
                    .SelectMany(newEntity => new[] {
                        new EntityModel(newEntity, mergeToSolution).GetGenerateInfo(),
                        new EntityViewModel(new View(), newEntity, mergeToSolution).GetGenerateInfo()
                    });
                await _codeGenerator.UpgradeAsync(mergeToSolution, oldGenerateInfos, newGenerateInfos);
            }
        }

        public void MergeEntity(Entity fromEntity, Entity toEntity, Entity mergeToEntity)
        {
            if (fromEntity.Name != toEntity.Name) mergeToEntity.Name = fromEntity.Name;
            if (fromEntity.PluralName != toEntity.PluralName) mergeToEntity.PluralName = fromEntity.PluralName;
            if (fromEntity.DisplayName != toEntity.DisplayName) mergeToEntity.DisplayName = fromEntity.DisplayName;
            if (fromEntity.PluralDisplayName != toEntity.PluralDisplayName) mergeToEntity.PluralDisplayName = fromEntity.PluralDisplayName;
            if (fromEntity.IsAbstract != toEntity.IsAbstract) mergeToEntity.IsAbstract = fromEntity.IsAbstract;
            if (fromEntity.BaseEntityId != toEntity.BaseEntityId) mergeToEntity.BaseEntityId = fromEntity.BaseEntityId;
            foreach (var field in fromEntity.Fields)
            {
                var toField = toEntity.Fields.FirstOrDefault(f => f.Id == field.Id);
                var newMergeField = mergeToEntity.Fields.FirstOrDefault(f => f.Id == field.Id);
                if (toField == null)
                {
                    mergeToEntity.Fields.Add(field);
                }
                else
                {
                    MergeField(field, toField, newMergeField);
                }
            }
            foreach (var toField in toEntity.Fields.ToList())
            {
                var field = fromEntity.Fields.FirstOrDefault(f => f.Id == toField.Id);
                var newMergeField = mergeToEntity.Fields.FirstOrDefault(f => f.Id == toField.Id);
                if (field == null)
                {
                    toEntity.Fields.Remove(toField);
                    if (newMergeField != null)
                    {
                        mergeToEntity.Fields.Remove(newMergeField);
                    }
                }
            }
            var fromProperties = fromEntity.Properties.SerializeToJson();
            var toProperties = toEntity.Properties.SerializeToJson();
            if (fromProperties != toProperties) mergeToEntity.Properties = fromProperties.DeserializeFromJson<PropertyCollection>();
        }

        private void MergeField(Field fromField, Field toField, Field mergeToField)
        {
            if (fromField.Name != toField.Name) mergeToField.Name = fromField.Name;
            if (fromField.DisplayName != toField.DisplayName) mergeToField.DisplayName = fromField.DisplayName;
            if (fromField.Type != toField.Type) mergeToField.Type = fromField.Type;
            if (fromField.Size != toField.Size) mergeToField.Size = fromField.Size;
            if (fromField.EntityId != toField.EntityId) mergeToField.EntityId = fromField.EntityId;
            if (fromField.IsRequired != toField.IsRequired) mergeToField.IsRequired = fromField.IsRequired;
            if (fromField.EnumId != toField.EnumId) mergeToField.EnumId = fromField.EnumId;
            if (fromField.IsSystem != toField.IsSystem) mergeToField.IsSystem = fromField.IsSystem;
            var fromProperties = fromField.Properties.SerializeToJson();
            var toProperties = toField.Properties.SerializeToJson();
            if (fromProperties != toProperties) mergeToField.Properties = fromProperties.DeserializeFromJson<PropertyCollection>();
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

        public async Task DeleteEntityAsync(SolutionAccessor solution, Entity entity)
        {
            var model = new EntityModel(entity, solution);
            await _codeGenerator.DeleteAsync(solution, model.GetGenerateInfo());
        }

        public async Task DeleteEntityViewAsync(SolutionAccessor solution, Entity entity)
        {
            var views = await solution.GetViewsAsync();
            foreach (var view in views)
            {
                await DeleteEntityViewAsync(solution, entity, view);
            }
        }

        public async Task DeleteEntityViewAsync(SolutionAccessor solution, Entity entity, View view)
        {
            var model = new EntityViewModel(view, entity, solution);
            await _codeGenerator.DeleteAsync(solution, model.GetGenerateInfo());
        }
    }
}
