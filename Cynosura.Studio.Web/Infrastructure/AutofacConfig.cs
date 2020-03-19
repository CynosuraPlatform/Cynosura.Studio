using Autofac;
using AutoMapper;
using Cynosura.Studio.Core.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Cynosura.Studio.Web.Infrastructure
{
    public class AutofacConfig
    {
        public static void ConfigureAutofac(ContainerBuilder builder, IConfiguration configuration)
        {
            var assemblies = CoreHelper.GetPlatformAndAppAssemblies();
            builder.RegisterAssemblyModules(assemblies);
            builder.Register(c => new MapperConfiguration(cfg => { cfg.AddMaps(assemblies); }).CreateMapper())
                .As<IMapper>().SingleInstance();
        }

    }
}
