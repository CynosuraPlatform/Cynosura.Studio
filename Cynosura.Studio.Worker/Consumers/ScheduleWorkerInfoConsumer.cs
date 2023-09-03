using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit;
using Cynosura.Studio.Core.Messaging.WorkerInfos;
using Cynosura.Studio.Worker.WorkerInfos;

namespace Cynosura.Studio.Worker.Consumers
{
    public class ScheduleWorkerInfoConsumer : IConsumer<ScheduleWorkerInfo>
    {
        private readonly WorkerInfoSheduler _workerInfoSheduler;

        public ScheduleWorkerInfoConsumer(WorkerInfoSheduler workerInfoSheduler)
        {
            _workerInfoSheduler = workerInfoSheduler;
        }

        public async Task Consume(ConsumeContext<ScheduleWorkerInfo> context)
        {
            await _workerInfoSheduler.ScheduleAsync(context.Message.Id);
        }
    }
}
