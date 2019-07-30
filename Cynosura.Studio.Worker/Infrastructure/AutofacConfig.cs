using System;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Cynosura.EF;
using Microsoft.Extensions.Configuration;
using Cynosura.Studio.Core.Autofac;
using Cynosura.Studio.Core.Security;
using Cynosura.Studio.Data;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Worker.Infrastructure
{
    public class AutofacConfig
    {
        public static void ConfigureAutofac(ContainerBuilder builder, IConfiguration configuration)
        {
            var assemblies = CoreHelper.GetPlatformAndAppAssemblies();
            builder.RegisterAssemblyModules(assemblies);
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>()
                .WithParameter((p, c) => p.Name == "connectionString",
                    (p, c) => configuration.GetConnectionString("DefaultConnection"))
                .InstancePerLifetimeScope();

            builder.RegisterType<UserInfoProvider>().As<IUserInfoProvider>().InstancePerLifetimeScope();
            builder.Register(c => new MapperConfiguration(cfg => { cfg.AddProfiles(assemblies); }).CreateMapper())
                .As<IMapper>().SingleInstance();
        }
    }
}
