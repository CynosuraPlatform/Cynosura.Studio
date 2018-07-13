using Autofac;
using Cynosura.Core.Data;
using Cynosura.EF;

namespace Cynosura.Studio.Data.Autofac
{
    public class DataModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UnitOfWork>().As<IUnitOfWork>().InstancePerLifetimeScope();
            builder.RegisterType<DatabaseInitializer>().As<IDatabaseInitializer>();
        }
    }
}
