using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Roles;
using Cynosura.Studio.Core.Requests.Roles.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleModel>();
            CreateMap<CreateRole, Role>();
            CreateMap<UpdateRole, Role>();
        }
    }
}
