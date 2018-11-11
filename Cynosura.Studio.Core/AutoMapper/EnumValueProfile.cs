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
            CreateMap<EnumValue, EnumValueModel>();
            CreateMap<CreateEnumValue, EnumValue>();
            CreateMap<UpdateEnumValue, EnumValue>();
        }
    }
}
