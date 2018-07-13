using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;
using Cynosura.Studio.Web.Models.UserViewModels;

namespace Cynosura.Studio.Web.Infrastructure.AutoMapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserViewModel>();
            CreateMap<CreateUserViewModel, UserCreateModel>();
            CreateMap<UpdateUserViewModel, UserUpdateModel>();
        }
    }
}
