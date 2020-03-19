using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cynosura.Web.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Cynosura.Studio.Web.Authorization
{
    public class RoleModule : IPolicyModule
    {
        public void RegisterPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("ReadRole",
                policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
            options.AddPolicy("WriteRole",
                policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
        }
    }
}
