using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using Cynosura.Core.Data;
using Cynosura.EF;
using Cynosura.Studio.Core.Entities;

namespace Cynosura.Studio.Data.Autofac
{
    public class UserModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<EntityRepository<User>>().As<IEntityRepository<User>>();
        }
    }
}
