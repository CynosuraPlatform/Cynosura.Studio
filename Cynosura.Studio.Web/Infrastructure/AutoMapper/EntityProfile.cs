using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;
using Cynosura.Studio.Web.Models.EntityViewModels;

namespace Cynosura.Studio.Web.Infrastructure.AutoMapper
{
    public class EntityProfile : Profile
    {
		public EntityProfile()
		{
			CreateMap<Entity, EntityViewModel>();
			CreateMap<EntityUpdateViewModel, EntityUpdateModel>();
			CreateMap<EntityCreateViewModel, EntityCreateModel>();
		}
    }
}
