using System;
using MediatR;

namespace Cynosura.Studio.Core.Requests.WorkerRuns
{
    public class DeleteWorkerRun : IRequest
    {
        public int Id { get; set; }
    }
}
