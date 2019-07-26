using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography.X509Certificates;
using AspNet.Security.OpenIdConnect.Primitives;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Cynosura.Studio.Core.Entities;
using Cynosura.Studio.Core.Infrastructure;
using Cynosura.Studio.Generator.PackageFeed.Models;
using Cynosura.Studio.Generator.PackageFeed;
using Cynosura.Studio.Data;
using Cynosura.Studio.Web.Infrastructure;
using Cynosura.Web;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Cynosura.Studio.Web
{
    public class Startup
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;

            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(_hostingEnvironment.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{hostingEnvironment.EnvironmentName}.json", optional: true, reloadOnChange: true)
                .AddJsonFile("appsettings.local.json", optional: true, reloadOnChange: true);
            configurationBuilder.AddEnvironmentVariables();

            Configuration = configurationBuilder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            services.Configure<NugetSettings>(Configuration.GetSection("Nuget"));
            services.Configure<LocalFeedOptions>(Configuration.GetSection("LocalFeed"));
            services.Configure<TemplateJsonProviderOptions>(Configuration.GetSection("Templates"));

            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
                options.UseOpenIddict();
            });

            services.AddIdentity<User, Role>()
                .AddEntityFrameworkStores<DataContext>()
                .AddDefaultTokenProviders();

            // Configure Identity to use the same JWT claims as OpenIddict instead
            // of the legacy WS-Federation claims it uses by default (ClaimTypes),
            // which saves you from doing the mapping in your authorization controller.
            services.Configure<IdentityOptions>(options =>
            {
                options.ClaimsIdentity.UserNameClaimType = OpenIdConnectConstants.Claims.Name;
                options.ClaimsIdentity.UserIdClaimType = OpenIdConnectConstants.Claims.Subject;
                options.ClaimsIdentity.RoleClaimType = OpenIdConnectConstants.Claims.Role;
            });

            // Register the OpenIddict services.
            services.AddOpenIddict()
                .AddCore(options =>
                {
                    // Configure OpenIddict to use the Entity Framework Core stores and entities.
                    options.UseEntityFrameworkCore()
                        .UseDbContext<DataContext>();
                })
                .AddServer(options =>
                {
                    // Register the ASP.NET Core MVC binder used by OpenIddict.
                    // Note: if you don't call this method, you won't be able to
                    // bind OpenIdConnectRequest or OpenIdConnectResponse parameters.
                    options.UseMvc();

                    // Enable the token endpoint (required to use the password flow).
                    options.EnableTokenEndpoint("/connect/token");

                    // Allow client applications to use the grant_type=password flow.
                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();

                    // During development, you can disable the HTTPS requirement.
                    options.DisableHttpsRequirement();

                    // Accept token requests that don't specify a client_id.
                    options.AcceptAnonymousClients();

                    options.UseJsonWebTokens();

                    if (_hostingEnvironment.IsDevelopment())
                    {
                        options.AddEphemeralSigningKey();
                    }
                    else
                    {
                        var certificate = new X509Certificate2(Configuration["Jwt:CertificatePath"],
                            Configuration["Jwt:CertificatePassword"]);
                        options.AddSigningCertificate(certificate);
                    }
                });

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.Authority = Configuration["Jwt:Authority"];
                    options.Audience = Configuration["Jwt:Audience"];
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        NameClaimType = OpenIdConnectConstants.Claims.Name,
                        RoleClaimType = OpenIdConnectConstants.Claims.Role,
                    };
                    if (_hostingEnvironment.IsDevelopment())
                    {
                        options.TokenValidationParameters.ValidateIssuer = false;
                        options.TokenValidationParameters.SignatureValidator = (token, parameters) =>
                            new JwtSecurityTokenHandler().ReadToken(token);
                    }
                });

            services.AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1)
                .AddMvcOptions(o =>
                {
                    o.Filters.Add(typeof(ExceptionLoggerFilter), 10);
                    o.ModelBinderProviders.Insert(0, new UserInfoModelBinderProvider());
                })
                .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
                        options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                        options.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
                        options.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
                    }
                );

            services.AddCors();

            var builder = new ContainerBuilder();
            AutofacConfig.ConfigureAutofac(builder, Configuration);
            builder.Populate(services);
            var applicationContainer = builder.Build();
            return new AutofacServiceProvider(applicationContainer);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseCors(builder =>
            {
                builder.WithOrigins(Configuration["Cors:Origin"])
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller}/{action=Index}/{id?}");
            });
        }
    }
}
