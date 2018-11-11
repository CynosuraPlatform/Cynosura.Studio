using System.Threading.Tasks;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.EnumValues;
using Cynosura.Studio.Core.Requests.EnumValues.Models;
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
    public class EnumValueController : Controller
    {
        private readonly IMediator _mediator;

        public EnumValueController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<PageModel<EnumValueModel>> GetEnumValuesAsync(int? pageIndex, int? pageSize)
        {
            return await _mediator.Send(new GetEnumValues() { PageIndex = pageIndex, PageSize = pageSize });
        }

        [HttpGet("{id:int}")]
        public async Task<EnumValueModel> GetEnumValueAsync(int id)
        {
            return await _mediator.Send(new GetEnumValue() { Id = id });
        }

        [HttpPut("{id:int}")]
        public async Task<StatusViewModel> PutEnumValueAsync(int id, [FromBody] UpdateEnumValue updateEnumValue)
        {
            await _mediator.Send(updateEnumValue);
            return new StatusViewModel();
        }

        [HttpPost("")]
        public async Task<StatusViewModel> PostEnumValueAsync([FromBody] CreateEnumValue createEnumValue)
        {
            var id = await _mediator.Send(createEnumValue);
            return new CreationStatusViewModel(id);
        }

        [HttpDelete("{id:int}")]
        public async Task<StatusViewModel> DeleteEnumValueAsync(int id)
        {
            await _mediator.Send(new DeleteEnumValue() { Id = id });
            return new StatusViewModel();
        }
    }
}