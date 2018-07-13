using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;
using Microsoft.AspNetCore.Identity;

namespace Cynosura.Studio.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<User> _userManager;
        private readonly IRoleService _roleService;
        private readonly RoleManager<Role> _roleManager;
        private readonly IEntityRepository<User> _userRepository;
        private readonly IMapper _mapper;

        public UserService(UserManager<User> userManager,
            IRoleService roleService,
            RoleManager<Role> roleManager,
            IEntityRepository<User> userRepository,
            IMapper mapper)
        {
            _userManager = userManager;
            _roleService = roleService;
            _roleManager = roleManager;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<User> GetUserAsync(int id)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            return user;
        }

        public async Task<PageModel<User>> GetUsersAsync(int? pageIndex = null, int? pageSize = null)
        {
            return await _userManager.Users
                .OrderBy(e => e.Id)
                .ToPagedListAsync(_userRepository, pageIndex, pageSize);
        }

        public async Task<int> CreateUserAsync(UserCreateModel model)
        {
            var user = BuildUser(model);
            var result = await _userManager.CreateAsync(user, model.Password);
            CheckResultSucceed(result);

            if (model.RoleIds.Any())
            {
                foreach (var roleId in model.RoleIds)
                {
                    var getRole = await _roleService.GetRoleAsync(roleId);
                    result = await _userManager.AddToRoleAsync(user, getRole.ToString());
                    CheckResultSucceed(result);
                }
            }
            return user.Id;
        }

        public async Task UpdateUserAsync(int id, UserUpdateModel model)
        {
            var user = await GetUserAsync(id);
            if (user == null)
                return;

            _mapper.Map(model, user);
            await _userManager.UpdateAsync(user);

            if (model.Password != null)
            {
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                var result = await _userManager.ResetPasswordAsync(user, resetToken, model.Password);
                CheckResultSucceed(result);
            }

            var userCurrentRoles = await _userManager.GetRolesAsync(user);
            var newUserRolesList = new List<Role>();
            foreach (var role in model.RoleIds)
            {
                var newRole = await _roleManager.FindByIdAsync(role.ToString());
                newUserRolesList.Add(newRole);
            }

            var newUserRoleNamesList = newUserRolesList.Select(newUserRole => newUserRole.Name).ToList();

            var resultNewRole = await _userManager.AddToRolesAsync(user, newUserRoleNamesList.Except(userCurrentRoles));
            CheckResultSucceed(resultNewRole);

            var resultCurrentRole = await _userManager.RemoveFromRolesAsync(user, userCurrentRoles.Except(newUserRoleNamesList));
            CheckResultSucceed(resultCurrentRole);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserAsync(id);
            if (user == null)
                return;
            var result = await _userManager.DeleteAsync(user);
            CheckResultSucceed(result);
        }

        private User BuildUser(UserCreateModel model)
        {
            var user = _mapper.Map<UserCreateModel, User>(model);
            user.UserName = model.Email;
            return user;
        }

        private static void CheckResultSucceed(IdentityResult result)
        {
            if (result.Succeeded)
                return;

            var errorDescription = result.Errors.Aggregate("",
                (current, error) => current + error.Description + " \r\n ");
            throw new ServiceException($"{errorDescription}");
        }
    }
}
