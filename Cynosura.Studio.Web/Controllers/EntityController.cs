using System;
using System.Threading.Tasks;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.Entities;
using Cynosura.Studio.Core.Requests.Entities.Models;
using Cynosura.Studio.Web.Models;
using Cynosura.Web.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [Route("api/[controller]")]
    public class EntityController : Controller
    {
        private readonly IMediator _mediator;

        public EntityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<PageModel<EntityModel>> GetEntitiesAsync(int solutionId, int? pageIndex, int? pageSize)
        {
            return await _mediator.Send(new GetEntities() { SolutionId = solutionId, PageIndex = pageIndex, PageSize = pageSize });
        }

        [HttpGet("{id:Guid}")]
        public async Task<EntityModel> GetEntityAsync(int solutionId, Guid id)
        {
            return await _mediator.Send(new GetEntity() { SolutionId = solutionId, Id = id});
        }

        [HttpPut("{id:Guid}")]
        public async Task<StatusViewModel> PutEntityAsync(int solutionId, Guid id, [FromBody] UpdateEntity updateEntity)
        {
            updateEntity.SolutionId = solutionId;
            await _mediator.Send(updateEntity);
            return new StatusViewModel();
        }

        [HttpPost("")]
        public async Task<StatusViewModel> PostEntityAsync(int solutionId, [FromBody] CreateEntity createEntity)
        {
            createEntity.SolutionId = solutionId;
            await _mediator.Send(createEntity);
            return new StatusViewModel();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<StatusViewModel> DeleteEntityAsync(int solutionId, Guid id)
        {
            await _mediator.Send(new DeleteEntity() { SolutionId = solutionId, Id = id });
            return new StatusViewModel();
        }

        [HttpPost("{id:Guid}/generate")]
        public async Task<StatusViewModel> GenerateEntityAsync(int solutionId, Guid id)
        {
            await _mediator.Send(new GenerateEntity() { SolutionId = solutionId, Id = id });
            return new StatusViewModel();
        }
    }
}