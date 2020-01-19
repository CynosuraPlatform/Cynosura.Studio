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
            CreateMap<User, UserModel>();
            CreateMap<User, UserShortModel>();
            CreateMap<CreateUser, User>();
            CreateMap<UpdateUser, User>();
        }
    }
}
