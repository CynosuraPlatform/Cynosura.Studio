using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Cynosura.Core.Services.Models;
using Cynosura.Web.Infrastructure;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.Enums;
using Cynosura.Studio.Core.Requests.Enums.Models;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [ValidateModel]
    [Route("api")]
    public class EnumController : Controller
    {
        private readonly IMediator _mediator;

        public EnumController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("GetEnums")]
        public async Task<PageModel<EnumModel>> GetEnumsAsync([FromBody] GetEnums getEnums)
        {
            return await _mediator.Send(getEnums);
        }

        [HttpPost("GetEnum")]
        public async Task<EnumModel?> GetEnumAsync([FromBody] GetEnum getEnum)
        {
            return await _mediator.Send(getEnum);
        }

        [HttpPost("ExportEnums")]
        public async Task<FileResult> ExportEnumsAsync([FromBody] ExportEnums exportEnums)
        {
            var file = await _mediator.Send(exportEnums);
            return File(file.Content, file.ContentType, file.Name);
        }

        [HttpPost("UpdateEnum")]
        public async Task<Unit> UpdateEnumAsync([FromBody] UpdateEnum updateEnum)
        {
            return await _mediator.Send(updateEnum);
        }

        [HttpPost("CreateEnum")]
        public async Task<CreatedEntity<Guid>> CreateEnumAsync([FromBody] CreateEnum createEnum)
        {
            return await _mediator.Send(createEnum);
        }

        [HttpPost("DeleteEnum")]
        public async Task<Unit> DeleteEnumAsync([FromBody] DeleteEnum deleteEnum)
        {
            return await _mediator.Send(deleteEnum);
        }

        [HttpPost("GenerateEnum")]
        public async Task<Unit> GenerateEnumAsync([FromBody] GenerateEnum generateEnum)
        {
            return await _mediator.Send(generateEnum);
        }
    }
}
