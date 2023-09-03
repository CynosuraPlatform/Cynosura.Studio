using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Roles.Models;

namespace Cynosura.Studio.Core.Requests.Roles
{
    public class GetRoleHandler : IRequestHandler<GetRole, RoleModel?>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public GetRoleHandler(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<RoleModel?> Handle(GetRole request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            if (role == null)
            {
                return null;
            }
            return _mapper.Map<Role, RoleModel>(role);
        }
    }
}
