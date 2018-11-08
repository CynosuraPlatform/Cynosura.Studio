using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Data;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Roles.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Cynosura.Studio.Core.Requests.Roles
{
    public class GetRolesHandler : IRequestHandler<GetRoles, PageModel<RoleModel>>
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IEntityRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public GetRolesHandler(RoleManager<Role> roleManager, IEntityRepository<Role> roleRepository, IMapper mapper)
        {
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public async Task<PageModel<RoleModel>> Handle(GetRoles request, CancellationToken cancellationToken)
        {
            var roles = await _roleManager.Roles
                .OrderBy(e => e.Id)
                .ToPagedListAsync(_roleRepository, request.PageIndex, request.PageSize);
            return roles.Map<Role, RoleModel>(_mapper);
        }
    }
}
