using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class CreateEntityValidator : AbstractValidator<CreateEntity>
    {
        public CreateEntityValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty().WithName("Name");
            RuleFor(x => x.PluralName).MaximumLength(100).NotEmpty().WithName("Plural Name");
            RuleFor(x => x.DisplayName).MaximumLength(100).NotEmpty().WithName("Display Name");
            RuleFor(x => x.PluralDisplayName).MaximumLength(100).NotEmpty().WithName("Plural Display Name");
        }

    }
}
