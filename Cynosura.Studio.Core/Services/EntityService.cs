using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Generator;
using Cynosura.Studio.Core.Services.Models;

namespace Cynosura.Studio.Core.Services
{
    public class EntityService : IEntityService
    {
        private readonly CodeGenerator _codeGenerator;
        private readonly ISolutionService _solutionService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EntityService(CodeGenerator codeGenerator,
            ISolutionService solutionService,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _codeGenerator = codeGenerator;
            _solutionService = solutionService;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Entity> GetEntityAsync(int solutionId, Guid id)
        {
            var entities = await GetEntitiesAsync(solutionId);
            return entities.PageItems
                .FirstOrDefault(e => e.Id == id);
        }

        private async Task<List<Entity>> GetEntitiesFromSolutionAsync(string path)
        {
            var solution = new SolutionAccessor(path);
            var entities = await solution.GetEntitiesAsync();
            return entities.Select(_mapper.Map<Generator.Models.Entity, Entity>)
                .ToList();
        }

        private static PageModel<T> ToPageModel<T>(IList<T> list, int? pageIndex = null, int? pageSize = null)
        {
            if (pageIndex != null && pageSize != null)
            {
                var pageList = list.Skip(pageIndex.Value * pageSize.Value)
                    .Take(pageSize.Value)
                    .ToList();
                return new PageModel<T>(pageList, list.Count, pageIndex.Value);
            }
            else
            {
                return new PageModel<T>(list, list.Count, 0);
            }
        }

        public async Task<PageModel<Entity> > GetEntitiesAsync(int solutionId, int? pageIndex = null, int? pageSize = null)
        {
            var solution = await _solutionService.GetSolutionAsync(solutionId);
            var entities = await GetEntitiesFromSolutionAsync(solution.Path);
            return ToPageModel(entities, pageIndex, pageSize);
        }

        public async Task<Guid> CreateEntityAsync(int solutionId, EntityCreateModel model)
        {
            var solution = await _solutionService.GetSolutionAsync(solutionId);
            var solutionModel = new SolutionAccessor(solution.Path);
            var entity = _mapper.Map<EntityCreateModel, Entity>(model);
            entity.Id = Guid.NewGuid();
            var entityModel = _mapper.Map<Entity, Generator.Models.Entity>(entity);
            await solutionModel.CreateEntityAsync(entityModel);
            // reload Entity from Solution
            entityModel = (await solutionModel.GetEntitiesAsync())
                .FirstOrDefault(e => e.Id == entity.Id);
            await _codeGenerator.GenerateEntityAsync(solutionModel, entityModel);
            await _codeGenerator.GenerateViewAsync(solutionModel, new Generator.Models.View(), entityModel);
            return entity.Id;
        }

        public async Task UpdateEntityAsync(int solutionId, Guid id, EntityUpdateModel model)
        {
            var solution = await _solutionService.GetSolutionAsync(solutionId);
            var solutionModel = new SolutionAccessor(solution.Path);
            var entity = _mapper.Map<EntityUpdateModel, Entity>(model);
            entity.Id = id;
            var oldEntity = (await solutionModel.GetEntitiesAsync()).FirstOrDefault(e => e.Id == id);
            var newEntity = _mapper.Map<Entity, Generator.Models.Entity>(entity);
            await solutionModel.UpdateEntityAsync(newEntity);
            // reload Entity from Solution
            newEntity = (await solutionModel.GetEntitiesAsync())
                .FirstOrDefault(e => e.Id == entity.Id);
            await _codeGenerator.UpgradeEntityAsync(solutionModel, oldEntity, newEntity);
            await _codeGenerator.UpgradeViewAsync(solutionModel, new Generator.Models.View(), oldEntity, newEntity);
        }

        public async Task DeleteEntityAsync(int solutionId, Guid id)
        {
            var solution = await _solutionService.GetSolutionAsync(solutionId);
            var solutionModel = new SolutionAccessor(solution.Path);
            await solutionModel.DeleteEntityAsync(id);
        }

        public async Task GenerateAsync(int solutionId, Guid id)
        {
            var solution = await _solutionService.GetSolutionAsync(solutionId);
            var solutionModel = new SolutionAccessor(solution.Path);
            var entity = await GetEntityAsync(solutionId, id);
            var entityModel = _mapper.Map<Entity, Generator.Models.Entity>(entity);
            await _codeGenerator.GenerateEntityAsync(solutionModel, entityModel);
            await _codeGenerator.GenerateViewAsync(solutionModel, new Generator.Models.View(), entityModel);
        }
    }
}
