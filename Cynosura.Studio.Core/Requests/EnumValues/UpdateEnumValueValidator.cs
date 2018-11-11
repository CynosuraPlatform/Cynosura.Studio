using FluentValidation;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class UpdateEnumValueValidator : AbstractValidator<UpdateEnumValue>
    {
        public UpdateEnumValueValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty();
            RuleFor(x => x.DisplayName).MaximumLength(100);
            RuleFor(x => x.Value);
            RuleFor(x => x.EnumId).NotEmpty();
        }

    }
}
