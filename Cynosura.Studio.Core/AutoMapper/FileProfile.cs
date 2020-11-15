using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.Files;
using Cynosura.Studio.Core.Requests.Files.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class FileProfile : Profile
    {
        public FileProfile()
        {
            CreateMap<File, FileModel>();
            CreateMap<File, FileShortModel>();
            CreateMap<CreateFile, File>()
                .ForMember(d => d.Content, o => o.Ignore());
            CreateMap<UpdateFile, File>()
                .ForMember(d => d.Content, o => o.Ignore());
        }
    }
}
