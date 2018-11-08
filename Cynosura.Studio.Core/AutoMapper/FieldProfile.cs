using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Fields;
using Cynosura.Studio.Core.Requests.Fields.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class FieldProfile : Profile
    {
        public FieldProfile()
        {
            CreateMap<Generator.Models.Field, FieldModel>();
            CreateMap<CreateField, Generator.Models.Field>();
            CreateMap<UpdateField, Generator.Models.Field>();
        }
    }
}
