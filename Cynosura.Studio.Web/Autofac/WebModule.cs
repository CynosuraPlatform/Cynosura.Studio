using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Cynosura.Studio.Core.Security;
using Cynosura.Studio.Web.Infrastructure;
using Cynosura.Web.Infrastructure;

namespace Cynosura.Studio.Web.Autofac
{
    class WebModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<ValidationExceptionHandler>().As<IExceptionHandler>();
            builder.RegisterType<StudioExceptionHandler>().As<IExceptionHandler>();
            builder.RegisterType<UserInfoProvider>().As<IUserInfoProvider>().InstancePerLifetimeScope();
        }
    }
}
