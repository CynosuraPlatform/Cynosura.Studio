using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Requests.Solutions;
using Cynosura.Studio.Core.Requests.Solutions.Models;
using Cynosura.Studio.Web.Protos.Solutions;

namespace Cynosura.Studio.Web.AutoMapper
{
    public class SolutionProfile : Profile
    {
        public SolutionProfile()
        {
            CreateMap<CreateSolutionRequest, CreateSolution>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.NameOneOfCase == CreateSolutionRequest.NameOneOfOneofCase.Name))
                .ForMember(dest => dest.Path, opt => opt.Condition(src => src.PathOneOfCase == CreateSolutionRequest.PathOneOfOneofCase.Path));
            CreateMap<DeleteSolutionRequest, DeleteSolution>();
            CreateMap<GetSolutionRequest, GetSolution>();
            CreateMap<GetSolutionsRequest, GetSolutions>()
                .ForMember(dest => dest.PageIndex, opt => opt.Condition(src => src.PageIndexOneOfCase == GetSolutionsRequest.PageIndexOneOfOneofCase.PageIndex))
                .ForMember(dest => dest.PageSize, opt => opt.Condition(src => src.PageSizeOneOfCase == GetSolutionsRequest.PageSizeOneOfOneofCase.PageSize))
                .ForMember(dest => dest.OrderDirection, opt => opt.Condition(src => src.OrderDirectionOneOfCase == GetSolutionsRequest.OrderDirectionOneOfOneofCase.OrderDirection));
            CreateMap<UpdateSolutionRequest, UpdateSolution>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.NameOneOfCase == UpdateSolutionRequest.NameOneOfOneofCase.Name))
                .ForMember(dest => dest.Path, opt => opt.Condition(src => src.PathOneOfCase == UpdateSolutionRequest.PathOneOfOneofCase.Path));

            CreateMap<SolutionModel, Solution>()
                .ForMember(dest => dest.Name, opt => opt.Condition(src => src.Name != default))
                .ForMember(dest => dest.Path, opt => opt.Condition(src => src.Path != default));
            CreateMap<PageModel<SolutionModel>, SolutionPageModel>()                
                .ForMember(dest => dest.PageItems, opt => opt.Ignore())
                .AfterMap((src, dest, rc) => dest.PageItems.AddRange(rc.Mapper.Map<IEnumerable<Solution>>(src.PageItems)));
        }
    }
}
