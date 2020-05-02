using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Profile
{
    public class UpdateProfileValidator : AbstractValidator<UpdateProfile>
    {
        public UpdateProfileValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();

            RuleFor(x => x.NewPassword)
                .Length(6, 100)
                .When(x => !string.IsNullOrEmpty(x.NewPassword));

            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .When(x => !string.IsNullOrEmpty(x.NewPassword));

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.NewPassword)
                .WithMessage("Password confirmation is not equal to password")
                .When(x => !string.IsNullOrEmpty(x.NewPassword));
        }
    }
}
