using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cynosura.Studio.Core.Workers
{
    public interface IWorker
    {
        Task ExecuteAsync(WorkerContext workerContext);
    }
}
