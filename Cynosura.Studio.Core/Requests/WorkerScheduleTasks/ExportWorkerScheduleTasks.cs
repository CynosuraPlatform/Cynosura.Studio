using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.WorkerScheduleTasks.Models;

namespace Cynosura.Studio.Core.Requests.WorkerScheduleTasks
{
    public class ExportWorkerScheduleTasks : IRequest<FileContentModel>
    {
        public WorkerScheduleTaskFilter? Filter { get; set; }
        public string? OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
