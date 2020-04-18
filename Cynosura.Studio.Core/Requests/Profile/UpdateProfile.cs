using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MediatR;

namespace Cynosura.Studio.Core.Requests.Profile
{
    public class UpdateProfile : IRequest
    {
        [DisplayName("Email")]
        public string Email { get; set; }
        [DisplayName("Current password")]
        public string CurrentPassword { get; set; }
        [DisplayName("New password")]
        public string NewPassword { get; set; }
        [DisplayName("Confirm password")]
        public string ConfirmPassword { get; set; }
    }
}
