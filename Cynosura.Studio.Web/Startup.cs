using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cynosura.Studio.Core;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Generator.PackageFeed.Models;
using Cynosura.Studio.Generator.PackageFeed;
using Cynosura.Studio.Data;
using Cynosura.Studio.Infrastructure;
using Cynosura.Studio.Web.Infrastructure;
using Cynosura.Web;
using Cynosura.Web.Authorization;
using Cynosura.Web.Infrastructure;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Cynosura.Studio.Generator;
using ElectronNET.API;

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

            services.AddMvc()
                .AddMvcOptions(o =>
                {
                    o.Filters.Add(typeof(ExceptionLoggerFilter), 10);
                    o.ModelBinderProviders.Insert(0, new UserInfoModelBinderProvider());
                })
                .AddJsonOptions(o =>
                {
                    o.JsonSerializerOptions.IgnoreNullValues = true;
                    o.JsonSerializerOptions.Converters.Add(new TimeSpanConverter());
                });

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

            app.UseStaticFiles();
            if (!env.IsDevelopment() || HybridSupport.IsElectronActive)
            {
                app.UseSpaStaticFiles();
            }

            app.UseRouting();

            app.UseCors(builder =>
            {
                builder.WithOrigins(Configuration["Cors:Origin"])
                    .AllowAnyMethod()
                    .AllowAnyHeader();
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

            if (!env.IsDevelopment() || HybridSupport.IsElectronActive) { 
                app.UseSpa(spa =>
                {
                });
            }
            Task.Run(async () => await Electron.WindowManager.CreateWindowAsync());
        }
    }
}
