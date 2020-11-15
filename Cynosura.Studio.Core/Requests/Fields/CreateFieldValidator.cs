using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Fields
{
    public class CreateFieldValidator : AbstractValidator<CreateField>
    {
        public CreateFieldValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty().Matches(RegexHelper.CSharpName).WithName(x => localizer["Name"]);
            RuleFor(x => x.DisplayName).MaximumLength(100).NotEmpty().WithName(x => localizer["Display Name"]);
            RuleFor(x => x.Type).NotEmpty().When(x => x.EntityId == null && x.EnumId == null).WithName(x => localizer["Type"]);
            RuleFor(x => x.Type).Empty().When(x => x.EntityId != null || x.EnumId != null).WithName(x => localizer["Type"]);
            RuleFor(x => x.EntityId).NotEmpty().When(x => x.Type == null && x.EnumId == null).WithName(x => localizer["Entity"]);
            RuleFor(x => x.EntityId).Empty().When(x => x.Type != null || x.EnumId != null).WithName(x => localizer["Entity"]);
            RuleFor(x => x.EnumId).NotEmpty().When(x => x.Type == null && x.EntityId == null).WithName(x => localizer["Enum"]);
            RuleFor(x => x.EnumId).Empty().When(x => x.Type != null || x.EntityId != null).WithName(x => localizer["Enum"]);
        }

    }
}
