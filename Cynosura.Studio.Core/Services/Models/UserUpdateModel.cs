using System.Collections.Generic;

namespace Cynosura.Studio.Core.Services.Models
{
    public class UserUpdateModel
    {
        public string Password { get; set; }
        public IEnumerable<int> RoleIds { get; set; }
    }
}
