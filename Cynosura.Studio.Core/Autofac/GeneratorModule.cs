using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Autofac;
using Cynosura.Studio.Core.Generator;
using Cynosura.Studio.Core.Infrastructure.Options;
using Cynosura.Studio.Core.Merge;
using Cynosura.Studio.Core.PackageFeed;
using Cynosura.Studio.Core.TemplateEngine;
using Microsoft.Extensions.Options;

namespace Cynosura.Studio.Core.Autofac
{
    public class GeneratorModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<CodeGenerator>();
            builder.RegisterType<StringTemplateEngine>().As<ITemplateEngine>();

            builder.RegisterType<LocalFeed>();
            builder.RegisterType<NugetFeed>();
            builder.Register(ctx =>
            {
                var options = ctx.Resolve<IOptions<LocalFeedOptions>>();
                return string.IsNullOrEmpty(options.Value?.SourcePath)
                    ? (IPackageFeed) (ctx.Resolve<NugetFeed>())
                    : ctx.Resolve<LocalFeed>();
            });

            builder.RegisterType<DmpMerge>().As<IMerge>();
            builder.RegisterType<FileMerge>();
        }
    }
}