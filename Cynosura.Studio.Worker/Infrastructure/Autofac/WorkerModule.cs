using Autofac;
using Microsoft.Extensions.Hosting;

namespace Cynosura.Studio.Worker.Infrastructure.Autofac
{
    class WorkerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<MainWorker>().As<IHostedService>().SingleInstance();
            builder.RegisterType<CoreLogProvider>();
        }
    }
}
