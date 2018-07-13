using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;
using Cynosura.Studio.Web.Models.RoleViewModels;

namespace Cynosura.Studio.Web.Infrastructure.AutoMapper
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleViewModel>();
            CreateMap<RoleViewModel, RoleCreateModel>();
        }
    }
}
