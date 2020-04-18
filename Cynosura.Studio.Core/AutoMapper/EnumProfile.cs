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
            CreateMap<Generator.Models.Enum, EnumModel>();
            CreateMap<Generator.Models.Enum, EnumShortModel>();
            CreateMap<CreateEnum, Generator.Models.Enum>();
            CreateMap<UpdateEnum, Generator.Models.Enum>();
        }
    }
}
