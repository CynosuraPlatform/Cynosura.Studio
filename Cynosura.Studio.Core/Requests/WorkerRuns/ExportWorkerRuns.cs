using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.WorkerRuns.Models;

namespace Cynosura.Studio.Core.Requests.WorkerRuns
{
    public class ExportWorkerRuns : IRequest<FileContentModel>
    {
        public WorkerRunFilter Filter { get; set; }
        public string OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
