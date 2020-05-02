using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.Enums;
using Cynosura.Studio.Core.Requests.Enums.Models;
using Cynosura.Studio.Web.Protos.Enums;

namespace Cynosura.Studio.Web.AutoMapper
{
    public class EnumProfile : Profile
    {
        public EnumProfile()
        {
            CreateMap<CreateEnumRequest, CreateEnum>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.NameOneOfCase == CreateEnumRequest.NameOneOfOneofCase.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.Condition(src => src.DisplayNameOneOfCase == CreateEnumRequest.DisplayNameOneOfOneofCase.DisplayName));
            CreateMap<DeleteEnumRequest, DeleteEnum>();
            CreateMap<GetEnumRequest, GetEnum>();
            CreateMap<GetEnumsRequest, GetEnums>()
                .ForMember(dest => dest.PageIndex, opt => opt.Condition(src => src.PageIndexOneOfCase == GetEnumsRequest.PageIndexOneOfOneofCase.PageIndex))
                .ForMember(dest => dest.PageSize, opt => opt.Condition(src => src.PageSizeOneOfCase == GetEnumsRequest.PageSizeOneOfOneofCase.PageSize))
                .ForMember(dest => dest.OrderDirection, opt => opt.Condition(src => src.OrderDirectionOneOfCase == GetEnumsRequest.OrderDirectionOneOfOneofCase.OrderDirection));
            CreateMap<UpdateEnumRequest, UpdateEnum>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.NameOneOfCase == UpdateEnumRequest.NameOneOfOneofCase.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.Condition(src => src.DisplayNameOneOfCase == UpdateEnumRequest.DisplayNameOneOfOneofCase.DisplayName));

            CreateMap<EnumModel, Protos.Enums.Enum>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != default))
                .ForMember(dest => dest.DisplayName, opt => opt.Condition(src => src.DisplayName != default));
            CreateMap<PageModel<EnumModel>, EnumPageModel>()                
                .ForMember(dest => dest.PageItems, opt => opt.Ignore())
                .AfterMap((src, dest, rc) => dest.PageItems.AddRange(rc.Mapper.Map<IEnumerable<Protos.Enums.Enum>>(src.PageItems)));
        }
    }
}
