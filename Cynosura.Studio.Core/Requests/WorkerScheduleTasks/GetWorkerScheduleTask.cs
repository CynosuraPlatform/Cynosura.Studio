using System;
using MediatR;
using Cynosura.Studio.Core.Requests.WorkerScheduleTasks.Models;

namespace Cynosura.Studio.Core.Requests.WorkerScheduleTasks
{
    public class GetWorkerScheduleTask : IRequest<WorkerScheduleTaskModel?>
    {
        public int Id { get; set; }
    }
}
