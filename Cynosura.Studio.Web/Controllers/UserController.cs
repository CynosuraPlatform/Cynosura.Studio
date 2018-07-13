using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services;
using Cynosura.Studio.Core.Services.Models;
using Cynosura.Studio.Web.Models;
using Cynosura.Studio.Web.Models.UserViewModels;
using Cynosura.Web.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Cynosura.Studio.Web.Controllers
{
    [ServiceFilter(typeof(ApiExceptionFilterAttribute))]
    [Authorize(Roles = "Administrator")]
    [ValidateModel]
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UserController(IUserService userService, IMapper mapper, UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            _userService = userService;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet("")]
        public async Task<PageModel<UserViewModel>> GetUsersAsync(int? pageIndex, int? pageSize)
        {
            var users = await _userService.GetUsersAsync(pageIndex, pageSize);
            return users.Map<User, UserViewModel>(_mapper);
        }

        [HttpGet("{id:int}")]
        public async Task<UserViewModel> GetUserAsync(int id)
        {
            var user = await _userService.GetUserAsync(id);
            var userRoleNames = await _userManager.GetRolesAsync(user);

            var roleIds = new List<int>();
            foreach (var roleName in userRoleNames)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                roleIds.Add(role.Id);
            }

            var model = _mapper.Map<User, UserViewModel>(user);
            model.RoleIds = roleIds;
            return model;
        }

        [HttpPut("{id:int}")]
        public async Task<StatusViewModel> PutUserAsync(int id, [FromBody] UpdateUserViewModel user)
        {
            var model = _mapper.Map<UpdateUserViewModel, UserUpdateModel>(user);
            await _userService.UpdateUserAsync(id, model);
            return new StatusViewModel();
        }

        [HttpPost("")]
        public async Task<StatusViewModel> PostUserAsync([FromBody] CreateUserViewModel user)
        {
            var userCreate = _mapper.Map<CreateUserViewModel, UserCreateModel>(user);
            var id = await _userService.CreateUserAsync(userCreate);
            return new CreationStatusViewModel(id);
        }

        [HttpDelete("{id:int}")]
        public async Task<StatusViewModel> DeleteUserAsync(int id)
        {
            await _userService.DeleteUserAsync(id);
            return new StatusViewModel();
        }
    }
}
