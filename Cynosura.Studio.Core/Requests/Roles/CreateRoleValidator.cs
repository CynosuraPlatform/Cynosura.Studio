using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Roles
{
    public class CreateRoleValidator : AbstractValidator<CreateRole>
    {
        public CreateRoleValidator()
        {
            RuleFor(x => x.Name).MaximumLength(256).NotEmpty();
        }

    }
}
