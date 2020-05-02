using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cynosura.Core.Data;
using Cynosura.EF;
using Cynosura.Studio.Core.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Cynosura.Studio.Data
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddData(this IServiceCollection services)
        {
            services.AddTransient<IDatabaseInitializer, DatabaseInitializer>();
            services.AddScoped<IDatabaseFactory, DatabaseFactory>();
            services.AddBaseEntityRepositories();
            services.AddCynosuraEF();
            return services;
        }

        private static void AddBaseEntityRepositories(this IServiceCollection services)
        {
            var type = typeof(BaseEntity);
            var baseEntities = type.Assembly
                .GetTypes()
                .Where(p => type.IsAssignableFrom(p) && p.IsClass)
                .ToList();
            foreach (var baseEntity in baseEntities)
            {
                var baseEntityRepositoryType = typeof(BaseEntityRepository<>).MakeGenericType(baseEntity);
                var entityRepositoryInterface = typeof(IEntityRepository<>).MakeGenericType(baseEntity);
                services.AddScoped(entityRepositoryInterface, baseEntityRepositoryType);
            }
        }
    }
}
