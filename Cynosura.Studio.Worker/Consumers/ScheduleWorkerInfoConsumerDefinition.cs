using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MassTransit.Definition;
using Cynosura.Studio.Core.Messaging.WorkerInfos;

namespace Cynosura.Studio.Worker.Consumers
{
    public class ScheduleWorkerInfoConsumerDefinition : ConsumerDefinition<ScheduleWorkerInfoConsumer>
    {
        public ScheduleWorkerInfoConsumerDefinition()
        {
            EndpointName = ScheduleWorkerInfo.QueueName;
        }
    }
}
