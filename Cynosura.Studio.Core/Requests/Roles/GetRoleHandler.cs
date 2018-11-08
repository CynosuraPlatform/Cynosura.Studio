using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Roles.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cynosura.Studio.Core.Requests.Roles
{
    public class GetRoleHandler : IRequestHandler<GetRole, RoleModel>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public GetRoleHandler(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<RoleModel> Handle(GetRole request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            return _mapper.Map<Role, RoleModel>(role);
        }
    }
}
