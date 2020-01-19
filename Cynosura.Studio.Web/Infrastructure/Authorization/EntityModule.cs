using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cynosura.Web.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Cynosura.Studio.Web.Infrastructure.Authorization
{
    public class EntityModule : IPolicyModule
    {
        public void RegisterPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("ReadEntity",
                policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
            options.AddPolicy("WriteEntity",
                policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
        }
    }
}