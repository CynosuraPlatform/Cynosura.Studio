using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.WorkerRuns;
using Cynosura.Studio.Core.Requests.WorkerRuns.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class WorkerRunProfile : Profile
    {
        public WorkerRunProfile()
        {
            CreateMap<WorkerRun, WorkerRunModel>();
            CreateMap<WorkerRun, WorkerRunShortModel>();
            CreateMap<CreateWorkerRun, WorkerRun>();
        }
    }
}
