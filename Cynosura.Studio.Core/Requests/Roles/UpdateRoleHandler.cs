using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cynosura.Studio.Core.Requests.Roles
{
    public class UpdateRoleHandler : IRequestHandler<UpdateRole>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IMapper _mapper;

        public UpdateRoleHandler(RoleManager<Role> roleManager, IMapper mapper)
        {
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task<Unit> Handle(UpdateRole request, CancellationToken cancellationToken)
        {
            var role = await _roleManager.FindByIdAsync(request.Id.ToString());
            _mapper.Map(request, role);
            var result = await _roleManager.UpdateAsync(role);
            result.CheckIfSucceeded();
            return Unit.Value;
        }
    }
}
