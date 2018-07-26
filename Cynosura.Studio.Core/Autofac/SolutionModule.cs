using Autofac;
using Cynosura.Studio.Core.Services;

namespace Cynosura.Studio.Core.Autofac
{
    public class SolutionModule : Module
    {
		protected override void Load(ContainerBuilder builder)
		{
			builder.RegisterType<SolutionService>().As<ISolutionService>();
		}
    }
}
