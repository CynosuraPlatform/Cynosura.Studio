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
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    public class EnumController : Controller
    {
        private readonly IMediator _mediator;

        public EnumController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<PageModel<EnumModel>> GetEnumsAsync(int? pageIndex, int? pageSize)
        {
            return await _mediator.Send(new GetEnums() { PageIndex = pageIndex, PageSize = pageSize });
        }

        [HttpGet("{id:int}")]
        public async Task<EnumModel> GetEnumAsync(int id)
        {
            return await _mediator.Send(new GetEnum() { Id = id });
        }

        [HttpPut("{id:int}")]
        public async Task<StatusViewModel> PutEnumAsync(int id, [FromBody] UpdateEnum updateEnum)
        {
            await _mediator.Send(updateEnum);
            return new StatusViewModel();
        }

        [HttpPost("")]
        public async Task<StatusViewModel> PostEnumAsync([FromBody] CreateEnum createEnum)
        {
            var id = await _mediator.Send(createEnum);
            return new CreationStatusViewModel(id);
        }

        [HttpDelete("{id:int}")]
        public async Task<StatusViewModel> DeleteEnumAsync(int id)
        {
            await _mediator.Send(new DeleteEnum() { Id = id });
            return new StatusViewModel();
        }
    }
}