using System;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Services.Models;
using Cynosura.Web.Infrastructure;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services;
using Cynosura.Studio.Core.Services.Models;
using Cynosura.Studio.Web.Models;
using Cynosura.Studio.Web.Models.EntityViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [ValidateModel]
    [Route("api/[controller]")]
    public class EntityController : Controller
    {
        private readonly IEntityService _entityService;
        private readonly IMapper _mapper;

        public EntityController(IEntityService entityService, IMapper mapper)
        {
            _entityService = entityService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<PageModel<EntityViewModel> > GetEntitiesAsync(int solutionId, int? pageIndex, int? pageSize)
        {
            var entities = await _entityService.GetEntitiesAsync(solutionId, pageIndex, pageSize);
            return entities.Map<Entity, EntityViewModel>(_mapper);
        }

        [HttpGet("{id:Guid}")]
        public async Task<EntityViewModel> GetEntityAsync(int solutionId, Guid id)
        {
            var entity = await _entityService.GetEntityAsync(solutionId, id);
            return _mapper.Map<Entity, EntityViewModel>(entity);
        }

        [HttpPut("{id:Guid}")]
        public async Task<StatusViewModel> PutEntityAsync(int solutionId, Guid id, [FromBody] EntityUpdateViewModel entity)
        {
            var model = _mapper.Map<EntityUpdateViewModel, EntityUpdateModel>(entity);
            await _entityService.UpdateEntityAsync(solutionId, id, model);
            return new StatusViewModel();
        }

        [HttpPost("")]
        public async Task<StatusViewModel> PostEntityAsync(int solutionId, [FromBody] EntityCreateViewModel entity)
        {
            var model = _mapper.Map<EntityCreateViewModel, EntityCreateModel>(entity);
            await _entityService.CreateEntityAsync(solutionId, model);
            return new StatusViewModel();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<StatusViewModel> DeleteEntityAsync(int solutionId, Guid id)
        {
            await _entityService.DeleteEntityAsync(solutionId, id);
            return new StatusViewModel();
        }

        [HttpPost("{id:Guid}/generate")]
        public async Task<StatusViewModel> GenerateSolutionAsync(int solutionId, Guid id)
        {
            await _entityService.GenerateAsync(solutionId, id);
            return new StatusViewModel();
        }
    }
}