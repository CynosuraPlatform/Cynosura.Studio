using Autofac;
using Cynosura.Studio.Core.Services;

namespace Cynosura.Studio.Core.Autofac
{
    public class UserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<UserService>().As<IUserService>();
        }
    }
}
