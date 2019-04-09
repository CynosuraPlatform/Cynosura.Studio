using FluentValidation;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class CreateEnumValueValidator : AbstractValidator<CreateEnumValue>
    {
        public CreateEnumValueValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty();
            RuleFor(x => x.DisplayName).MaximumLength(100);
            //RuleFor(x => x.Value);
        }

    }
}
