using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;
using Cynosura.Studio.Web.Models.ProjectViewModels;

namespace Cynosura.Studio.Web.Infrastructure.AutoMapper
{
    public class ProjectProfile : Profile
    {
		public ProjectProfile()
		{
			CreateMap<Project, ProjectViewModel>();
			CreateMap<ProjectUpdateViewModel, ProjectUpdateModel>();
			CreateMap<ProjectCreateViewModel, ProjectCreateModel>();
		}
    }
}
