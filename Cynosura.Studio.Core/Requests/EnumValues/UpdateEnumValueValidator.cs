using FluentValidation;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class UpdateEnumValueValidator : AbstractValidator<UpdateEnumValue>
    {
        public UpdateEnumValueValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty().Matches(RegexHelper.CSharpName);
            RuleFor(x => x.DisplayName).MaximumLength(100);
        }

    }
}
