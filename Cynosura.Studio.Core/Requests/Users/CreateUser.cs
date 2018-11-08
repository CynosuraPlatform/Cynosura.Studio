using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class CreateUser : IRequest<int>
    {
        public string Email { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public IList<int> RoleIds { get; } = new List<int>();
    }
}
