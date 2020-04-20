using System;
using System.Collections.Generic;
using System.Text;
using Cynosura.Studio.Generator.Merge;
using Cynosura.Studio.Generator.PackageFeed;
using Cynosura.Studio.Generator.PackageFeed.Models;
using Cynosura.Studio.Generator.TemplateEngine;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cynosura.Studio.Generator
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddGenerator(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<CodeGenerator>();
            services.AddTransient<SolutionGenerator>();
            services.AddTransient<EntityGenerator>();
            services.AddTransient<EnumGenerator>();
            services.AddTransient<ITemplateEngine, StringTemplateEngine>();

            services.AddTransient<LocalFeed>();
            services.AddTransient<NugetFeed>();
            services.AddTransient(sp =>
            {
                var options = sp.GetRequiredService<IOptions<LocalFeedOptions>>();
                return string.IsNullOrEmpty(options.Value?.SourcePath)
                    ? (IPackageFeed)(sp.GetService<NugetFeed>())
                    : sp.GetService<LocalFeed>();
            });

            services.AddTransient<IDirectoryMerge, GitMerge>();

            return services;
        }
    }
}
