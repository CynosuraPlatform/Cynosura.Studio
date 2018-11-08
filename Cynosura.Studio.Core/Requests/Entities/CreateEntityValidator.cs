using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class CreateEntityValidator : AbstractValidator<CreateEntity>
    {
        public CreateEntityValidator()
        {
            RuleFor(x => x.Name).Length(100).NotEmpty();
            RuleFor(x => x.PluralName).Length(100).NotEmpty();
            RuleFor(x => x.DisplayName).Length(100).NotEmpty();
            RuleFor(x => x.PluralDisplayName).Length(100).NotEmpty();
        }

    }
}
