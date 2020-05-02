using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.Entities;
using Cynosura.Studio.Core.Requests.Entities.Models;
using Cynosura.Studio.Web.Protos.Entities;

namespace Cynosura.Studio.Web.AutoMapper
{
    public class EntityProfile : Profile
    {
        public EntityProfile()
        {
            CreateMap<CreateEntityRequest, CreateEntity>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.NameOneOfCase == CreateEntityRequest.NameOneOfOneofCase.Name))
                .ForMember(dest => dest.PluralName, opt => opt.Condition(src => src.PluralNameOneOfCase == CreateEntityRequest.PluralNameOneOfOneofCase.PluralName))
                .ForMember(dest => dest.DisplayName, opt => opt.Condition(src => src.DisplayNameOneOfCase == CreateEntityRequest.DisplayNameOneOfOneofCase.DisplayName))
                .ForMember(dest => dest.PluralDisplayName, opt => opt.Condition(src => src.PluralDisplayNameOneOfCase == CreateEntityRequest.PluralDisplayNameOneOfOneofCase.PluralDisplayName))
                .ForMember(dest => dest.IsAbstract, opt => opt.Condition(src => src.IsAbstractOneOfCase == CreateEntityRequest.IsAbstractOneOfOneofCase.IsAbstract))
                .ForMember(dest => dest.BaseEntityId, opt => opt.Condition(src => src.BaseEntityIdOneOfCase == CreateEntityRequest.BaseEntityIdOneOfOneofCase.BaseEntityId));
            CreateMap<DeleteEntityRequest, DeleteEntity>();
            CreateMap<GetEntityRequest, GetEntity>();
            CreateMap<GetEntitiesRequest, GetEntities>()
                .ForMember(dest => dest.PageIndex, opt => opt.Condition(src => src.PageIndexOneOfCase == GetEntitiesRequest.PageIndexOneOfOneofCase.PageIndex))
                .ForMember(dest => dest.PageSize, opt => opt.Condition(src => src.PageSizeOneOfCase == GetEntitiesRequest.PageSizeOneOfOneofCase.PageSize))
                .ForMember(dest => dest.OrderDirection, opt => opt.Condition(src => src.OrderDirectionOneOfCase == GetEntitiesRequest.OrderDirectionOneOfOneofCase.OrderDirection));
            CreateMap<UpdateEntityRequest, UpdateEntity>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.NameOneOfCase == UpdateEntityRequest.NameOneOfOneofCase.Name))
                .ForMember(dest => dest.PluralName, opt => opt.Condition(src => src.PluralNameOneOfCase == UpdateEntityRequest.PluralNameOneOfOneofCase.PluralName))
                .ForMember(dest => dest.DisplayName, opt => opt.Condition(src => src.DisplayNameOneOfCase == UpdateEntityRequest.DisplayNameOneOfOneofCase.DisplayName))
                .ForMember(dest => dest.PluralDisplayName, opt => opt.Condition(src => src.PluralDisplayNameOneOfCase == UpdateEntityRequest.PluralDisplayNameOneOfOneofCase.PluralDisplayName))
                .ForMember(dest => dest.IsAbstract, opt => opt.Condition(src => src.IsAbstractOneOfCase == UpdateEntityRequest.IsAbstractOneOfOneofCase.IsAbstract))
                .ForMember(dest => dest.BaseEntityId, opt => opt.Condition(src => src.BaseEntityIdOneOfCase == UpdateEntityRequest.BaseEntityIdOneOfOneofCase.BaseEntityId));

            CreateMap<EntityModel, Entity>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != default))
                .ForMember(dest => dest.PluralName, opt => opt.Condition(src => src.PluralName != default))
                .ForMember(dest => dest.DisplayName, opt => opt.Condition(src => src.DisplayName != default))
                .ForMember(dest => dest.PluralDisplayName, opt => opt.Condition(src => src.PluralDisplayName != default))
                .ForMember(dest => dest.IsAbstract, opt => opt.Condition(src => src.IsAbstract != default))
                .ForMember(dest => dest.BaseEntityId, opt => opt.Condition(src => src.BaseEntityId != default));
            CreateMap<PageModel<EntityModel>, EntityPageModel>()                
                .ForMember(dest => dest.PageItems, opt => opt.Ignore())
                .AfterMap((src, dest, rc) => dest.PageItems.AddRange(rc.Mapper.Map<IEnumerable<Entity>>(src.PageItems)));
        }
    }
}
