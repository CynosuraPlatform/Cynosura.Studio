using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Cynosura.Web.Authorization;

namespace Cynosura.Studio.Web.Authorization
{
    public class ViewModule : IPolicyModule
    {
        public void RegisterPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("ReadView",
                policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
            options.AddPolicy("WriteView",
                policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
        }
    }
}