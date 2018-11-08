using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class UpdateUserHandler : IRequestHandler<UpdateUser>
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UpdateUserHandler(UserManager<User> userManager, RoleManager<Role> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateUser request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id.ToString());
            if (user != null)
            {
                _mapper.Map(request, user);
                var result = await _userManager.UpdateAsync(user);
                result.CheckIfSucceeded();

                if (request.Password != null)
                {
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    result = await _userManager.ResetPasswordAsync(user, resetToken, request.Password);
                    result.CheckIfSucceeded();
                }

                var userCurrentRoles = await _userManager.GetRolesAsync(user);
                var newUserRolesList = new List<Role>();
                foreach (var role in request.RoleIds)
                {
                    var newRole = await _roleManager.FindByIdAsync(role.ToString());
                    newUserRolesList.Add(newRole);
                }

                var newUserRoleNamesList = newUserRolesList.Select(newUserRole => newUserRole.Name).ToList();

                result = await _userManager.AddToRolesAsync(user, newUserRoleNamesList.Except(userCurrentRoles));
                result.CheckIfSucceeded();

                result = await _userManager.RemoveFromRolesAsync(user, userCurrentRoles.Except(newUserRoleNamesList));
                result.CheckIfSucceeded();
            }
            return Unit.Value;
        }
    }
}
