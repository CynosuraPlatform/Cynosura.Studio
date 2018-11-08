using System.Threading.Tasks;
using Cynosura.Core.Services.Models;
using Cynosura.Web.Infrastructure;
using Cynosura.Studio.Core.Requests.Roles;
using Cynosura.Studio.Core.Requests.Roles.Models;
using Cynosura.Studio.Web.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [Authorize(Roles = "Administrator")]
    [Route("api/[controller]")]
    public class RoleController : Controller
    {
        private readonly IMediator _mediator;

        public RoleController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<PageModel<RoleModel>> GetRolesAsync(int? pageIndex, int? pageSize)
        {
            return await _mediator.Send(new GetRoles() { PageIndex = pageIndex, PageSize = pageSize });
        }

        [HttpGet("{id:int}")]
        public async Task<RoleModel> GetRoleAsync(int id)
        {
            return await _mediator.Send(new GetRole() { Id = id });
        }

        [HttpPut("{id:int}")]
        public async Task<StatusViewModel> PutRoleAsync(int id, [FromBody] UpdateRole updateRole)
        {
            await _mediator.Send(updateRole);
            return new StatusViewModel();
        }

        [HttpPost("")]
        public async Task<StatusViewModel> PostRoleAsync([FromBody] CreateRole createRole)
        {
            var id = await _mediator.Send(createRole);
            return new CreationStatusViewModel(id);
        }

        [HttpDelete("{id:int}")]
        public async Task<StatusViewModel> DeleteRoleAsync(int id)
        {
            await _mediator.Send(new DeleteRole() { Id = id });
            return new StatusViewModel();
        }
    }
}