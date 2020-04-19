using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.Views;
using Cynosura.Studio.Core.Requests.Views.Models;
using Cynosura.Studio.Web.Protos.Views;

namespace Cynosura.Studio.Web.AutoMapper
{
    public class ViewProfile : Profile
    {
        public ViewProfile()
        {
            CreateMap<CreateViewRequest, CreateView>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.NameOneOfCase == CreateViewRequest.NameOneOfOneofCase.Name));
            CreateMap<DeleteViewRequest, DeleteView>();
            CreateMap<GetViewRequest, GetView>();
            CreateMap<GetViewsRequest, GetViews>()
                .ForMember(dest => dest.PageIndex, opt => opt.Condition(src => src.PageIndexOneOfCase == GetViewsRequest.PageIndexOneOfOneofCase.PageIndex))
                .ForMember(dest => dest.PageSize, opt => opt.Condition(src => src.PageSizeOneOfCase == GetViewsRequest.PageSizeOneOfOneofCase.PageSize))
                .ForMember(dest => dest.OrderDirection, opt => opt.Condition(src => src.OrderDirectionOneOfCase == GetViewsRequest.OrderDirectionOneOfOneofCase.OrderDirection));
            CreateMap<UpdateViewRequest, UpdateView>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.NameOneOfCase == UpdateViewRequest.NameOneOfOneofCase.Name));

            CreateMap<ViewModel, View>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != default));
            CreateMap<PageModel<ViewModel>, ViewPageModel>()                
                .ForMember(dest => dest.PageItems, opt => opt.Ignore())
                .AfterMap((src, dest, rc) => dest.PageItems.AddRange(rc.Mapper.Map<IEnumerable<View>>(src.PageItems)));
        }
    }
}
