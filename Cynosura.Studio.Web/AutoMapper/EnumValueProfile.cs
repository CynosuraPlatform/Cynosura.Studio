using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.EnumValues;
using Cynosura.Studio.Core.Requests.EnumValues.Models;
using Cynosura.Studio.Web.Protos.EnumValues;

namespace Cynosura.Studio.Web.AutoMapper
{
    public class EnumValueProfile : Profile
    {
        public EnumValueProfile()
        {
            CreateMap<CreateEnumValueRequest, CreateEnumValue>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.NameOneOfCase == CreateEnumValueRequest.NameOneOfOneofCase.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.Condition(src => src.DisplayNameOneOfCase == CreateEnumValueRequest.DisplayNameOneOfOneofCase.DisplayName))
                .ForMember(dest => dest.Value, opt => opt.Condition(src => src.ValueOneOfCase == CreateEnumValueRequest.ValueOneOfOneofCase.Value));
            CreateMap<UpdateEnumValueRequest, UpdateEnumValue>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.NameOneOfCase == UpdateEnumValueRequest.NameOneOfOneofCase.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.Condition(src => src.DisplayNameOneOfCase == UpdateEnumValueRequest.DisplayNameOneOfOneofCase.DisplayName))
                .ForMember(dest => dest.Value, opt => opt.Condition(src => src.ValueOneOfCase == UpdateEnumValueRequest.ValueOneOfOneofCase.Value));

            CreateMap<EnumValueModel, EnumValue>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != default))
                .ForMember(dest => dest.DisplayName, opt => opt.Condition(src => src.DisplayName != default))
                .ForMember(dest => dest.Value, opt => opt.Condition(src => src.Value != default));
            CreateMap<PageModel<EnumValueModel>, EnumValuePageModel>()                
                .ForMember(dest => dest.PageItems, opt => opt.Ignore())
                .AfterMap((src, dest, rc) => dest.PageItems.AddRange(rc.Mapper.Map<IEnumerable<EnumValue>>(src.PageItems)));
        }
    }
}
