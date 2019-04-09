using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class UpdateEnumValidator : AbstractValidator<UpdateEnum>
    {
        public UpdateEnumValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty();
            RuleFor(x => x.DisplayName).MaximumLength(100).NotEmpty();
        }

    }
}
