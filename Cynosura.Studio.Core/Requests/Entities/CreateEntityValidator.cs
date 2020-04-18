using Cynosura.Studio.Core.Requests.Fields;
using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class CreateEntityValidator : AbstractValidator<CreateEntity>
    {
        public CreateEntityValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty().Matches(RegexHelper.CSharpName);
            RuleFor(x => x.PluralName).MaximumLength(100).NotEmpty().Matches(RegexHelper.CSharpName);
            RuleFor(x => x.DisplayName).MaximumLength(100).NotEmpty();
            RuleFor(x => x.PluralDisplayName).MaximumLength(100).NotEmpty();
            RuleFor(x => x.BaseEntityId);
            RuleFor(x => x.IsAbstract);
            RuleForEach(x => x.Fields).SetValidator(new CreateFieldValidator());
        }

    }
}
