using System;
using Cynosura.Studio.Core.Infrastructure;

namespace Cynosura.Studio.Core.Requests.Users.Models
{
    public class UserFilter : EntityFilter
    {
        public string? UserName { get; set; }
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
