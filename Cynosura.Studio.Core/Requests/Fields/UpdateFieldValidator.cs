using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Fields
{
    public class UpdateFieldValidator : AbstractValidator<UpdateField>
    {
        public UpdateFieldValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty().Matches(RegexHelper.CSharpName);
            RuleFor(x => x.DisplayName).MaximumLength(100).NotEmpty();
            RuleFor(x => x.Type).NotEmpty().When(x => x.EntityId == null && x.EnumId == null);
            RuleFor(x => x.Type).Empty().When(x => x.EntityId != null || x.EnumId != null);
            RuleFor(x => x.EntityId).NotEmpty().When(x => x.Type == null && x.EnumId == null);
            RuleFor(x => x.EntityId).Empty().When(x => x.Type != null || x.EnumId != null);
            RuleFor(x => x.EnumId).NotEmpty().When(x => x.Type == null && x.EntityId == null);
            RuleFor(x => x.EnumId).Empty().When(x => x.Type != null || x.EntityId != null);
        }

    }
}
