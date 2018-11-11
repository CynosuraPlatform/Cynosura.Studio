using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Enums;
using Cynosura.Studio.Core.Requests.Enums.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class EnumProfile : Profile
    {
        public EnumProfile()
        {
            CreateMap<Enum, EnumModel>();
            CreateMap<CreateEnum, Enum>();
            CreateMap<UpdateEnum, Enum>();
        }
    }
}
