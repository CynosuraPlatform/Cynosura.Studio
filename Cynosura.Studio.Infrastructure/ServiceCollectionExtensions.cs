using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Cynosura.Studio.Core.Email;
using Cynosura.Studio.Core.Formatters;
using Cynosura.Studio.Infrastructure.Email;
using Cynosura.Studio.Infrastructure.Formatters;

namespace Cynosura.Studio.Infrastructure
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<IEmailSender, EmailSender>();
            services.Configure<EmailSenderOptions>(configuration.GetSection("EmailSender"));
            services.AddTransient<IExcelFormatter, ExcelFormatter>();
            return services;
        }
    }
}
