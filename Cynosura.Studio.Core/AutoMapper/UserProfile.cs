using System.Linq;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Users;
using Cynosura.Studio.Core.Requests.Users.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserModel>()
                .ForMember(dst => dst.RoleIds, opt => opt.MapFrom(src => src.Roles.Select(e => e.Id)));
            CreateMap<User, UserShortModel>();
            CreateMap<CreateUser, User>();
            CreateMap<UpdateUser, User>();
        }
    }
}
