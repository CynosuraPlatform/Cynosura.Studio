using System;
using MediatR;

namespace Cynosura.Studio.Core.Requests.WorkerScheduleTasks
{
    public class DeleteWorkerScheduleTask : IRequest
    {
        public int Id { get; set; }
    }
}
