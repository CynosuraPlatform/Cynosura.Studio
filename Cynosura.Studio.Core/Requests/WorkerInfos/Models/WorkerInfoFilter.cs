using System;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.WorkerInfos.Models
{
    public class WorkerInfoFilter : EntityFilter
    {
        public string Name { get; set; }
        public string ClassName { get; set; }
    }
}
