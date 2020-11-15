using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.FileGroups;
using Cynosura.Studio.Core.Requests.FileGroups.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class FileGroupProfile : Profile
    {
        public FileGroupProfile()
        {
            CreateMap<FileGroup, FileGroupModel>();
            CreateMap<FileGroup, FileGroupShortModel>();
            CreateMap<CreateFileGroup, FileGroup>();
            CreateMap<UpdateFileGroup, FileGroup>();
        }
    }
}
