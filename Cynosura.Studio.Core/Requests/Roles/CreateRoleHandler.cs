using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cynosura.Studio.Core.Requests.Roles
{
    public class CreateRoleHandler : IRequestHandler<CreateRole, int>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public CreateRoleHandler(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<int> Handle(CreateRole request, CancellationToken cancellationToken)
        {
            var role = _mapper.Map<CreateRole, Role>(request);
            var result = await _roleManager.CreateAsync(role);
            result.CheckIfSucceeded();
            return role.Id;
        }
    }
}
