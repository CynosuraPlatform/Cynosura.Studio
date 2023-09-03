using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Cynosura.Studio.Core.Messaging.WorkerRuns;
using Cynosura.Studio.Worker.WorkerInfos;

namespace Cynosura.Studio.Worker.Consumers
{
    public class StartWorkerRunConsumer : IConsumer<StartWorkerRun>
    {
        private readonly WorkerInfoSheduler _workerInfoSheduler;

        public StartWorkerRunConsumer(WorkerInfoSheduler workerInfoSheduler)
        {
            _workerInfoSheduler = workerInfoSheduler;
        }

        public async Task Consume(ConsumeContext<StartWorkerRun> context)
        {
            await _workerInfoSheduler.RunAsync(context.Message);
        }
    }
}
