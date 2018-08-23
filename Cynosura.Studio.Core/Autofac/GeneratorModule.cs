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
            builder.RegisterType<CodeGenerator>();
            builder.RegisterType<StringTemplateEngine>().As<ITemplateEngine>();
            builder.RegisterType<NugetFeed>().As<IPackageFeed>();
            builder.RegisterType<DmpMerge>().As<IMerge>();
        }
    }
}
