using System;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Roles.Models
{
    public class RoleFilter : EntityFilter
    {
        public string Name { get; set; }
        public string DisplayName { get; set; }
    }
}
