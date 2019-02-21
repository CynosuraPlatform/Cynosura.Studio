using FluentValidation;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class UpdateEnumValueValidator : AbstractValidator<UpdateEnumValue>
    {
        public UpdateEnumValueValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty().WithName("Name");
            RuleFor(x => x.DisplayName).MaximumLength(100).WithName("Display Name");
            //RuleFor(x => x.Value).WithName("Value");
        }

    }
}
