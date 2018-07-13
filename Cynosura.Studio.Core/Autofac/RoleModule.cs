using Autofac;
using Cynosura.Studio.Core.Services;

namespace Cynosura.Studio.Core.Autofac
{
    public class RoleModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<RoleService>().As<IRoleService>();
        }
    }
}
