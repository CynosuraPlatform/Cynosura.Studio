using Autofac;
using Cynosura.Core.Data;
using Cynosura.EF;
using Cynosura.Studio.Core.Entities;

namespace Cynosura.Studio.Data.Autofac
{
    public class SolutionModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<BaseEntityRepository<Solution>>().As<IEntityRepository<Solution>>();
        }
    }
}
