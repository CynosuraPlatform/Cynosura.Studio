using MediatR;
using Cynosura.Core.Services.Models;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.WorkerScheduleTasks.Models;

namespace Cynosura.Studio.Core.Requests.WorkerScheduleTasks
{
    public class GetWorkerScheduleTasks : IRequest<PageModel<WorkerScheduleTaskModel>>
    {
        public int? PageIndex { get; set; }
        public int? PageSize { get; set; }

        public WorkerScheduleTaskFilter? Filter { get; set; }
        public string? OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
