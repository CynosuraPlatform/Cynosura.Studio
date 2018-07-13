using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Cynosura.Studio.Web.Models.UserViewModels
{
    public class CreateUserViewModel : UpdateUserViewModel
    {
        [Required]
        [EmailAddress(ErrorMessage = "{0} has invalid format")]
        public string Email { get; set; }
    }
}
