using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Services.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class EntityProfile : Profile
    {
		public EntityProfile()
		{
			CreateMap<EntityCreateModel, Entity>();
			CreateMap<EntityUpdateModel, Entity>();
		}
    }
}
