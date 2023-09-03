using System;
using MediatR;

namespace Cynosura.Studio.Core.Requests.WorkerInfos
{
    public class DeleteWorkerInfo : IRequest
    {
        public int Id { get; set; }
    }
}
