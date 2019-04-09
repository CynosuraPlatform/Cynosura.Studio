using System.Threading.Tasks;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.Users;
using Cynosura.Studio.Core.Requests.Users.Models;
using Cynosura.Studio.Web.Models;
using Cynosura.Web.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [Authorize(Roles = "Administrator")]
    [ValidateModel]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("")]
        public async Task<PageModel<UserModel>> GetUsersAsync(int? pageIndex, int? pageSize, UserFilter filter)
        {
            return await _mediator.Send(new GetUsers() { PageIndex = pageIndex, PageSize = pageSize, Filter = filter });
        }

        [HttpGet("{id:int}")]
        public async Task<UserModel> GetUserAsync(int id)
        {
            return await _mediator.Send(new GetUser() { Id = id });
        }

        [HttpPut("{id:int}")]
        public async Task<StatusViewModel> PutUserAsync(int id, [FromBody] UpdateUser updateUser)
        {
            await _mediator.Send(updateUser);
            return new StatusViewModel();
        }

        [HttpPost("")]
        public async Task<StatusViewModel> PostUserAsync([FromBody] CreateUser createUser)
        {
            var id = await _mediator.Send(createUser);
            return new CreationStatusViewModel(id);
        }

        [HttpDelete("{id:int}")]
        public async Task<StatusViewModel> DeleteUserAsync(int id)
        {
            await _mediator.Send(new DeleteUser() { Id = id });
            return new StatusViewModel();
        }
    }
}
