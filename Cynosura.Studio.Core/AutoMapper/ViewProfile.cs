using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Views;
using Cynosura.Studio.Core.Requests.Views.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class ViewProfile : Profile
    {
        public ViewProfile()
        {
            CreateMap<Generator.Models.View, ViewModel>();
            CreateMap<Generator.Models.View, ViewShortModel>();
            CreateMap<CreateView, Generator.Models.View>();
            CreateMap<UpdateView, Generator.Models.View>();
        }
    }
}
