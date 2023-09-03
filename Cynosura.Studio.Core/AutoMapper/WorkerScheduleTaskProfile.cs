using AutoMapper;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Requests.WorkerScheduleTasks;
using Cynosura.Studio.Core.Requests.WorkerScheduleTasks.Models;

namespace Cynosura.Studio.Core.AutoMapper
{
    public class WorkerScheduleTaskProfile : Profile
    {
        public WorkerScheduleTaskProfile()
        {
            CreateMap<WorkerScheduleTask, WorkerScheduleTaskModel>();
            CreateMap<WorkerScheduleTask, WorkerScheduleTaskShortModel>();
            CreateMap<CreateWorkerScheduleTask, WorkerScheduleTask>();
            CreateMap<UpdateWorkerScheduleTask, WorkerScheduleTask>();
        }
    }
}
