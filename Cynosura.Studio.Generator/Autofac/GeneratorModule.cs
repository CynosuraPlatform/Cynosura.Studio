using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Autofac;
using Cynosura.Studio.Generator.Merge;
using Cynosura.Studio.Generator.PackageFeed;
using Cynosura.Studio.Generator.PackageFeed.Models;
using Cynosura.Studio.Generator.TemplateEngine;
using Microsoft.Extensions.Options;

namespace Cynosura.Studio.Generator.Autofac
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

            builder.RegisterType<DmpMerge>().As<IFileMerge>();
            builder.RegisterType<DirectoryMerge>().As<IDirectoryMerge>();
        }
    }
}
