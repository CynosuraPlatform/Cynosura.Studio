using Autofac;
using Cynosura.Studio.Core.Services;

namespace Cynosura.Studio.Core.Autofac
{
    public class ProjectModule : Module
    {
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<ProjectService>().As<IProjectService>();
		}
    }
}
