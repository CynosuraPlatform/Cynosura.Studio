using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class UpdateUser : IRequest
    {
        public int Id { get; set; }

        [DisplayName("Password")]
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public List<int> RoleIds { get; set; }
    }
}
