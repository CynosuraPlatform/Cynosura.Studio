using Autofac;
using Cynosura.Studio.Core.Services;

namespace Cynosura.Studio.Core.Autofac
{
    public class EntityModule : Module
    {
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<EntityService>().As<IEntityService>();
		}
    }
}
