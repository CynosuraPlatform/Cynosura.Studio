using System;
using MediatR;
using Cynosura.Studio.Core.Requests.WorkerInfos.Models;

namespace Cynosura.Studio.Core.Requests.WorkerInfos
{
    public class GetWorkerInfo : IRequest<WorkerInfoModel?>
    {
        public int Id { get; set; }
    }
}
