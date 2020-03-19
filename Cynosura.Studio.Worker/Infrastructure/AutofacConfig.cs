using Autofac;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Worker.Infrastructure
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
