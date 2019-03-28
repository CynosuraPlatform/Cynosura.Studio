using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Quartz;
using Quartz.Simpl;
using Quartz.Spi;

namespace Cynosura.Studio.Worker.Infrastructure
{
    public class AutofacJobFactory : SimpleJobFactory
    {
        private readonly Dictionary<IJob, ILifetimeScope> _scopes = new Dictionary<IJob, ILifetimeScope>();
        private readonly ILifetimeScope _lifetimeScope;

        public AutofacJobFactory(ILifetimeScope lifetimeScope)
        {
            _lifetimeScope = lifetimeScope;
        }

        public override IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            try
            {
                var scope = _lifetimeScope.BeginLifetimeScope();
                var job = (IJob)scope.Resolve(bundle.JobDetail.JobType);
                _scopes[job] = scope;
                return job;
            }
            catch (Exception e)
            {
                throw new SchedulerException(
                    $"Problem while instantiating job '{bundle.JobDetail.Key}' from the AutofacJobFactory.", e);
            }
        }

        public override void ReturnJob(IJob job)
        {
            var scope = _scopes[job];
            _scopes.Remove(job);
            base.ReturnJob(job);
            scope.Dispose();
        }
    }
}
