using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Cynosura.Web.Authorization;

namespace Cynosura.Studio.Web.Authorization
{
    public class FieldModule : IPolicyModule
    {
        public void RegisterPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("ReadField",
                policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
            options.AddPolicy("WriteField",
                policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
        }
    }
}