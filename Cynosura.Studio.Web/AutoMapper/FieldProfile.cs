using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.Fields;
using Cynosura.Studio.Core.Requests.Fields.Models;
using Cynosura.Studio.Web.Protos.Fields;

namespace Cynosura.Studio.Web.AutoMapper
{
    public class FieldProfile : Profile
    {
        public FieldProfile()
        {
            CreateMap<CreateFieldRequest, CreateField>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.NameOneOfCase == CreateFieldRequest.NameOneOfOneofCase.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.Condition(src => src.DisplayNameOneOfCase == CreateFieldRequest.DisplayNameOneOfOneofCase.DisplayName))
                .ForMember(dest => dest.Size, opt => opt.Condition(src => src.SizeOneOfCase == CreateFieldRequest.SizeOneOfOneofCase.Size))
                .ForMember(dest => dest.EntityId, opt => opt.Condition(src => src.EntityIdOneOfCase == CreateFieldRequest.EntityIdOneOfOneofCase.EntityId))
                .ForMember(dest => dest.IsRequired, opt => opt.Condition(src => src.IsRequiredOneOfCase == CreateFieldRequest.IsRequiredOneOfOneofCase.IsRequired))
                .ForMember(dest => dest.EnumId, opt => opt.Condition(src => src.EnumIdOneOfCase == CreateFieldRequest.EnumIdOneOfOneofCase.EnumId))
                .ForMember(dest => dest.IsSystem, opt => opt.Condition(src => src.IsSystemOneOfCase == CreateFieldRequest.IsSystemOneOfOneofCase.IsSystem));
            CreateMap<UpdateFieldRequest, UpdateField>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.NameOneOfCase == UpdateFieldRequest.NameOneOfOneofCase.Name))
                .ForMember(dest => dest.DisplayName, opt => opt.Condition(src => src.DisplayNameOneOfCase == UpdateFieldRequest.DisplayNameOneOfOneofCase.DisplayName))
                .ForMember(dest => dest.Size, opt => opt.Condition(src => src.SizeOneOfCase == UpdateFieldRequest.SizeOneOfOneofCase.Size))
                .ForMember(dest => dest.EntityId, opt => opt.Condition(src => src.EntityIdOneOfCase == UpdateFieldRequest.EntityIdOneOfOneofCase.EntityId))
                .ForMember(dest => dest.IsRequired, opt => opt.Condition(src => src.IsRequiredOneOfCase == UpdateFieldRequest.IsRequiredOneOfOneofCase.IsRequired))
                .ForMember(dest => dest.EnumId, opt => opt.Condition(src => src.EnumIdOneOfCase == UpdateFieldRequest.EnumIdOneOfOneofCase.EnumId))
                .ForMember(dest => dest.IsSystem, opt => opt.Condition(src => src.IsSystemOneOfCase == UpdateFieldRequest.IsSystemOneOfOneofCase.IsSystem));

            CreateMap<FieldModel, Field>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != default))
                .ForMember(dest => dest.DisplayName, opt => opt.Condition(src => src.DisplayName != default))
                .ForMember(dest => dest.Size, opt => opt.Condition(src => src.Size != default))
                .ForMember(dest => dest.EntityId, opt => opt.Condition(src => src.EntityId != default))
                .ForMember(dest => dest.IsRequired, opt => opt.Condition(src => src.IsRequired != default))
                .ForMember(dest => dest.EnumId, opt => opt.Condition(src => src.EnumId != default))
                .ForMember(dest => dest.IsSystem, opt => opt.Condition(src => src.IsSystem != default));
            CreateMap<PageModel<FieldModel>, FieldPageModel>()                
                .ForMember(dest => dest.PageItems, opt => opt.Ignore())
                .AfterMap((src, dest, rc) => dest.PageItems.AddRange(rc.Mapper.Map<IEnumerable<Field>>(src.PageItems)));
        }
    }
}
