using System;
using MediatR;
using Cynosura.Studio.Core.Requests.WorkerRuns.Models;

namespace Cynosura.Studio.Core.Requests.WorkerRuns
{
    public class GetWorkerRun : IRequest<WorkerRunModel>
    {
        public int Id { get; set; }
    }
}
