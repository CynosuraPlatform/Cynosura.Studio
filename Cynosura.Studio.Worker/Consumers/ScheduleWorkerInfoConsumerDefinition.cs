using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit.Definition;
using Cynosura.Studio.Core.Messaging.WorkerInfos;

namespace Cynosura.Studio.Worker.Consumers
{
    class ScheduleWorkerInfoConsumerDefinition : ConsumerDefinition<ScheduleWorkerInfoConsumer>
    {
        protected ScheduleWorkerInfoConsumerDefinition()
        {
            EndpointName = ScheduleWorkerInfo.QueueName;
        }
    }
}
