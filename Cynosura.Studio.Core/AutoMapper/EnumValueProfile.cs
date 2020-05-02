using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.EnumValues;
using Cynosura.Studio.Core.Requests.EnumValues.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class EnumValueProfile : Profile
    {
        public EnumValueProfile()
        {
            CreateMap<Generator.Models.EnumValue, EnumValueModel>();
            CreateMap<Generator.Models.EnumValue, EnumValueShortModel>();
            CreateMap<CreateEnumValue, Generator.Models.EnumValue>();
            CreateMap<UpdateEnumValue, Generator.Models.EnumValue>();
        }
    }
}
