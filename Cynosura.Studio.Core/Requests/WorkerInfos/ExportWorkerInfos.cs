using MediatR;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Requests.WorkerInfos.Models;

namespace Cynosura.Studio.Core.Requests.WorkerInfos
{
    public class ExportWorkerInfos : IRequest<FileContentModel>
    {
        public WorkerInfoFilter? Filter { get; set; }
        public string? OrderBy { get; set; }
        public OrderDirection? OrderDirection { get; set; }
    }
}
