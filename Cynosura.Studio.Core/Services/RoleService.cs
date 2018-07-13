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
    public class RoleService : IRoleService
    {
        private readonly RoleManager<Role> _roleManager;
        private readonly IEntityRepository<Role> _roleRepository;
        private readonly IMapper _mapper;

        public RoleService(RoleManager<Role> roleManager, IEntityRepository<Role> roleRepository, 
            IMapper mapper)
        {
            _roleManager = roleManager;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        public Task<Role> GetRoleAsync(int id)
        {
            return _roleManager.FindByIdAsync(id.ToString());
        }

        public async Task<PageModel<Role>> GetRolesAsync(int? pageIndex = null, int? pageSize = null)
        {
            return await _roleManager.Roles
                .OrderBy(e => e.Id)
                .ToPagedListAsync(_roleRepository, pageIndex, pageSize);
        }

        public async Task<int> CreateRoleAsync(RoleCreateModel model)
        {
            var role = _mapper.Map<RoleCreateModel, Role>(model);
            var result = await _roleManager.CreateAsync(role);
            CheckResultSucceed(result);
            return role.Id;
        }

        public async Task UpdateRoleAsync(int id, RoleUpdateModel model)
        {
            var role = await GetRoleAsync(id);
            _mapper.Map(model, role);
            var result = await _roleManager.UpdateAsync(role);
            CheckResultSucceed(result);
        }

        public async Task DeleteRoleAsync(int id)
        {
            var role = await GetRoleAsync(id);
            if (role == null)
                return;
            var result = await _roleManager.DeleteAsync(role);
            CheckResultSucceed(result);
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
