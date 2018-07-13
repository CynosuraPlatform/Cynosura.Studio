using System.Collections.Generic;

namespace Cynosura.Studio.Web.Models.UserViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public IEnumerable<int> RoleIds { get; set; }
    }
}
