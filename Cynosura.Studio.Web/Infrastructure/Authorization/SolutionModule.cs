using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Cynosura.Web.Infrastructure.Authorization;
using Microsoft.AspNetCore.Authorization;

namespace Cynosura.Studio.Web.Infrastructure.Authorization
{
    public class SolutionModule : IPolicyModule
    {
        public void RegisterPolicies(AuthorizationOptions options)
        {
            options.AddPolicy("ReadSolution",
                policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
            options.AddPolicy("WriteSolution",
                policy => policy.RequireClaim(ClaimTypes.Role, "Administrator"));
        }
    }
}