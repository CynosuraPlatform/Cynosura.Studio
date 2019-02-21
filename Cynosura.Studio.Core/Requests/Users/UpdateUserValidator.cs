using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Users
{
    public class UpdateUserValidator : AbstractValidator<UpdateUser>
    {
        public UpdateUserValidator()
        {
            RuleFor(x => x.Password).Length(6, 100).WithName("Password");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("Passwords do not match");
        }
    }
}
