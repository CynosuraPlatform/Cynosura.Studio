using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Solutions;
using Cynosura.Studio.Core.Requests.Solutions.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class SolutionProfile : Profile
    {
        public SolutionProfile()
        {
            CreateMap<Solution, SolutionModel>();
            CreateMap<CreateSolution, Solution>();
            CreateMap<UpdateSolution, Solution>();
            CreateMap<OpenSolution, Solution>();
        }
    }
}
