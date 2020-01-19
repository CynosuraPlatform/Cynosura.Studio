using System;
using System.Collections.Generic;
using System.Text;
using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Account
{
    public class RegisterValidator : AbstractValidator<Register>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email).EmailAddress().NotEmpty();
            RuleFor(x => x.Password).Length(6, 100).NotEmpty();
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Password confirmation is not equal to password");
        }
    }
}
