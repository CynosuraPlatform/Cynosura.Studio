using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Cynosura.Studio.Core.Generator;
using Cynosura.Studio.Core.Merge;
using Cynosura.Studio.Core.PackageFeed;
using Cynosura.Studio.Core.TemplateEngine;

namespace Cynosura.Studio.Core.Autofac
{
    public class GeneratorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            builder.RegisterType<CodeGenerator>();
            builder.RegisterType<StringTemplateEngine>().As<ITemplateEngine>();
            if (environment == "Development")
            {
                builder.RegisterType<LocalFeed>().As<IPackageFeed>();
            }
            else
            {
                builder.RegisterType<NugetFeed>().As<IPackageFeed>();
            }
            builder.RegisterType<DmpMerge>().As<IMerge>();
            builder.RegisterType<FileMerge>();
        }
    }
}
