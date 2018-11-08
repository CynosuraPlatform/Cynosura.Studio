using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Entities;
using Cynosura.Studio.Core.Requests.Entities.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class EntityProfile : Profile
    {
		public EntityProfile()
		{
			CreateMap<Generator.Models.Entity, EntityModel>();
            CreateMap<CreateEntity, Generator.Models.Entity>();
			CreateMap<UpdateEntity, Generator.Models.Entity>();
        }
    }
}
