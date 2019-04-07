using System;
using System.Threading.Tasks;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.Enums;
using Cynosura.Studio.Core.Requests.Enums.Models;
using Cynosura.Studio.Web.Models;
using Cynosura.Web.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [ValidateModel]
    [Route("api/[controller]")]
    public class EnumController : Controller
    {
        private readonly IMediator _mediator;

        public EnumController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<PageModel<EnumModel>> GetEnumsAsync(int solutionId, int? pageIndex, int? pageSize, EnumFilter filter)
        {
            return await _mediator.Send(new GetEnums() { SolutionId = solutionId, PageIndex = pageIndex, PageSize = pageSize, Filter = filter });
        }

        [HttpGet("{id:Guid}")]
        public async Task<EnumModel> GetEnumAsync(int solutionId, Guid id)
        {
            return await _mediator.Send(new GetEnum() { SolutionId = solutionId, Id = id });
        }

        [HttpPut("{id:Guid}")]
        public async Task<StatusViewModel> PutEnumAsync(int solutionId, Guid id, [FromBody] UpdateEnum updateEnum)
        {
            updateEnum.SolutionId = solutionId;
            await _mediator.Send(updateEnum);
            return new StatusViewModel();
        }

        [HttpPost("")]
        public async Task<StatusViewModel> PostEnumAsync(int solutionId, [FromBody] CreateEnum createEnum)
        {
            createEnum.SolutionId = solutionId;
            await _mediator.Send(createEnum);
            return new StatusViewModel();
        }

        [HttpDelete("{id:Guid}")]
        public async Task<StatusViewModel> DeleteEnumAsync(int solutionId, Guid id)
        {
            await _mediator.Send(new DeleteEnum() { SolutionId = solutionId, Id = id });
            return new StatusViewModel();
        }

        [HttpPost("{id:Guid}/generate")]
        public async Task<StatusViewModel> GenerateEnumAsync(int solutionId, Guid id)
        {
            await _mediator.Send(new GenerateEnum() { SolutionId = solutionId, Id = id });
            return new StatusViewModel();
        }
    }
}