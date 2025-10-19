using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cynosura.Core.Services.Models;
using Cynosura.Web.Infrastructure;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Entities;
using Cynosura.Studio.Core.Requests.Entities.Models;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [ValidateModel]
    [Route("api")]
    public class EntityController : Controller
    {
        private readonly IMediator _mediator;

        public EntityController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GetEntities")]
        public async Task<PageModel<EntityModel>> GetEntitiesAsync(int solutionId, [FromBody] GetEntities getEntities)
        {
            return await _mediator.Send(getEntities);
        }

        [HttpPost("GetEntity")]
        public async Task<EntityModel?> GetEntityAsync([FromBody] GetEntity getEntity)
        {
            return await _mediator.Send(getEntity);
        }

        [HttpPost("ExportEntities")]
        public async Task<FileResult> ExportEntitiesAsync([FromBody] ExportEntities exportEntities)
        {
            var file = await _mediator.Send(exportEntities);
            return File(file.Content, file.ContentType, file.Name);
        }

        [HttpPost("UpdateEntity")]
        public async Task UpdateEntityAsync([FromBody] UpdateEntity updateEntity)
        {
            await _mediator.Send(updateEntity);
        }

        [HttpPost("CreateEntity")]
        public async Task<CreatedEntity<Guid>> CreateEntityAsync([FromBody] CreateEntity createEntity)
        {
            return await _mediator.Send(createEntity);
        }

        [HttpPost("DeleteEntity")]
        public async Task DeleteEntityAsync([FromBody] DeleteEntity deleteEntity)
        {
            await _mediator.Send(deleteEntity);
        }

        [HttpPost("GenerateEntity")]
        public async Task GenerateEntityAsync([FromBody] GenerateEntity generateEntity)
        {
            await _mediator.Send(generateEntity);
        }
    }
}
