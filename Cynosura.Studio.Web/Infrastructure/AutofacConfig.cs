using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Cynosura.EF;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Core.Security;
using Cynosura.Studio.Data;
using Cynosura.Web.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Cynosura.Studio.Web.Infrastructure
{
    public class AutofacConfig
    {
        public static void ConfigureAutofac(ContainerBuilder builder, IConfiguration configuration)
        {
            var assemblies = CoreHelper.GetPlatformAndAppAssemblies();
            builder.RegisterAssemblyModules(assemblies);
            builder.RegisterType<DatabaseFactory>().As<IDatabaseFactory>()
                .WithParameter((p, c) => p.Name == "connectionString", (p, c) => configuration.GetConnectionString("DefaultConnection"))
                .InstancePerLifetimeScope();

            builder.RegisterType<ValidationExceptionHandler>().As<IExceptionHandler>();
            builder.RegisterType<StudioExceptionHandler>().As<IExceptionHandler>();
            builder.RegisterType<UserInfoProvider>().As<IUserInfoProvider>().InstancePerLifetimeScope();
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                cfg.AddMaps(assemblies);
            }).CreateMapper()).As<IMapper>().SingleInstance();
        }

    }
}
