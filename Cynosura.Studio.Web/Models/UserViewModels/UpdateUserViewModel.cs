using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cynosura.Studio.Web.Models.UserViewModels
{
    public class UpdateUserViewModel
    {
        [StringLength(100, ErrorMessage = "{0} must contain between {2} and {1} characters", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        public IEnumerable<int> RoleIds { get; set; }
    }
}
