using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.WorkerInfos;
using Cynosura.Studio.Core.Requests.WorkerInfos.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class WorkerInfoProfile : Profile
    {
        public WorkerInfoProfile()
        {
            CreateMap<WorkerInfo, WorkerInfoModel>();
            CreateMap<WorkerInfo, WorkerInfoShortModel>();
            CreateMap<CreateWorkerInfo, WorkerInfo>();
            CreateMap<UpdateWorkerInfo, WorkerInfo>();
        }
    }
}
