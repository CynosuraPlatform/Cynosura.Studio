using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.Json.Serialization;
using MicroElements.Swashbuckle.FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Cynosura.Web;
using Cynosura.Web.Authorization;
using Cynosura.Web.Infrastructure;
using Cynosura.Studio.Core;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Data;
using Cynosura.Studio.Generator;
using Cynosura.Studio.Generator.PackageFeed;
using Cynosura.Studio.Generator.PackageFeed.Models;
using Cynosura.Studio.Infrastructure;
using Cynosura.Studio.Web.Infrastructure;

namespace Cynosura.Studio.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<NugetSettings>(Configuration.GetSection("Nuget"));
            services.Configure<LocalFeedOptions>(Configuration.GetSection("LocalFeed"));
            services.Configure<TemplateJsonProviderOptions>(Configuration.GetSection("Templates"));
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(Configuration.GetConnectionString("DefaultConnection"));
            });

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DataContext>();

            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "NgApp";
            });

            services.AddPortableObjectLocalization(options => options.ResourcesPath = "Localization");

            services.AddMvc()
                .AddMvcOptions(o =>
                {
                    o.Filters.Add(typeof(ExceptionLoggerFilter), 10);
                    o.ModelBinderProviders.Insert(0, new UserInfoModelBinderProvider());
                })
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
                    o.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                })
                .AddViewLocalization(LanguageViewLocationExpanderFormat.Suffix)
                .AddDataAnnotationsLocalization();
            services.AddRazorPages();

            services.AddAuthorization(options =>
            {
                new PolicyProvider().RegisterPolicies(options);
            });

            services.AddCors();

            services.AddGrpc();

            services.AddWeb(Configuration);
            services.AddInfrastructure(Configuration);
            services.AddData();
            services.AddCore(Configuration);
            services.AddGenerator(Configuration);
            services.AddCynosuraWeb();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            var supportedCultures = new[]
            {
                new CultureInfo("en-US"),
                new CultureInfo("ru-RU")
            };
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture("en-US"),
                SupportedCultures = supportedCultures,
                SupportedUICultures = supportedCultures
            });

            app.UseStaticFiles();
            if (!env.IsDevelopment())
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseCors(builder =>
            {
                builder.WithOrigins(Configuration["Cors:Origin"])
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithExposedHeaders("Content-Disposition");
            });

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
                var provider = new ConfigurationProvider<IEndpointRouteBuilder>();
                provider.Configure(endpoints);
            });

            if (!env.IsDevelopment()) { 
                app.UseSpa(spa =>
                {
                });
            }
        }
    }
}
