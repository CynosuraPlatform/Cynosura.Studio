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
        public async Task<PageModel<EntityViewModel> > GetEntitiesAsync(int projectId, int? pageIndex, int? pageSize)
        {
            var entities = await _entityService.GetEntitiesAsync(projectId, pageIndex, pageSize);
            return entities.Map<Entity, EntityViewModel>(_mapper);
        }

        [HttpGet("{id:int}")]
        public async Task<EntityViewModel> GetEntityAsync(int projectId, Guid id)
        {
            var entity = await _entityService.GetEntityAsync(projectId, id);
            return _mapper.Map<Entity, EntityViewModel>(entity);
        }

        [HttpPut("{id:int}")]
        public async Task<StatusViewModel> PutEntityAsync(int projectId, Guid id, [FromBody] EntityUpdateViewModel entity)
        {
            var model = _mapper.Map<EntityUpdateViewModel, EntityUpdateModel>(entity);
            await _entityService.UpdateEntityAsync(projectId, id, model);
            return new StatusViewModel();
        }

        [HttpPost("")]
        public async Task<StatusViewModel> PostEntityAsync(int projectId, [FromBody] EntityCreateViewModel entity)
        {
            var model = _mapper.Map<EntityCreateViewModel, EntityCreateModel>(entity);
            await _entityService.CreateEntityAsync(projectId, model);
            return new StatusViewModel();
        }

        [HttpDelete("{id:int}")]
        public async Task<StatusViewModel> DeleteEntityAsync(int projectId, Guid id)
        {
            await _entityService.DeleteEntityAsync(projectId, id);
            return new StatusViewModel();
        }
    }
}