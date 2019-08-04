using System;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using Cynosura.Studio.Core.Autofac;
using Cynosura.Studio.Generator.Autofac;
using Microsoft.Extensions.Configuration;

namespace Cynosura.Studio.CliTool
{
    public class AutofacConfig
    {
        public static void ConfigureAutofac(ContainerBuilder builder, IConfiguration configuration)
        {
            var assemblies = GetPlatformAndAppAssemblies();
            builder.RegisterModule<CoreModule>();
            builder.RegisterModule<GeneratorModule>();
            builder.Register(c => new MapperConfiguration(cfg =>
            {
                cfg.AddProfiles(assemblies);
            }).CreateMapper()).As<IMapper>().SingleInstance();
        }

        private static Assembly[] GetPlatformAndAppAssemblies()
        {
            var platformAndAppNames = new[] { "Cynosura", "Cynosura.Studio" };
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => platformAndAppNames.Any(n => a.FullName.Contains(n)) ||
                            a.GetReferencedAssemblies()
                                .Any(ra => platformAndAppNames.Any(n => ra.FullName.Contains(n))))
                .ToArray();
        }
    }
}
