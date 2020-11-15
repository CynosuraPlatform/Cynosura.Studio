using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.EntityChanges.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class EntityChangeProfile : Profile
    {
        public EntityChangeProfile()
        {
            CreateMap<EntityChange, EntityChangeModel>();
        }
    }
}
