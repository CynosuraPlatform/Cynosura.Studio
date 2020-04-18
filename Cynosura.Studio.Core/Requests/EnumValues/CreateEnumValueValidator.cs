using FluentValidation;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class CreateEnumValueValidator : AbstractValidator<CreateEnumValue>
    {
        public CreateEnumValueValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty().Matches(RegexHelper.CSharpName);
            RuleFor(x => x.DisplayName).MaximumLength(100);
        }

    }
}
