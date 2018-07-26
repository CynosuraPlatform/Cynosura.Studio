using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;
using Cynosura.Studio.Web.Models.SolutionViewModels;

namespace Cynosura.Studio.Web.Infrastructure.AutoMapper
{
    public class SolutionProfile : Profile
    {
		public SolutionProfile()
		{
			CreateMap<Solution, SolutionViewModel>();
			CreateMap<SolutionUpdateViewModel, SolutionUpdateModel>();
			CreateMap<SolutionCreateViewModel, SolutionCreateModel>();
		}
    }
}
