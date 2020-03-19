using Autofac;
using Cynosura.Studio.Core.Security;
using Cynosura.Studio.Worker.Infrastructure;
using Microsoft.Extensions.Hosting;

namespace Cynosura.Studio.Worker.Autofac
{
    class WorkerModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserInfoProvider>().As<IUserInfoProvider>().InstancePerLifetimeScope();
            builder.RegisterType<MainWorker>().As<IHostedService>().SingleInstance();
            builder.RegisterType<CoreLogProvider>();
        }
    }
}
