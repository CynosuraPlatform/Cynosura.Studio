using FluentValidation;
using Microsoft.Extensions.Localization;
﻿using Cynosura.Studio.Core.Requests.Fields;

namespace Cynosura.Studio.Core.Requests.Entities
{
    public class UpdateEntityValidator : AbstractValidator<UpdateEntity>
    {
        public UpdateEntityValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty().Matches(RegexHelper.CSharpName).WithName(x => localizer["Name"]);
            RuleFor(x => x.PluralName).MaximumLength(100).NotEmpty().Matches(RegexHelper.CSharpName).WithName(x => localizer["Plural Name"]);
            RuleFor(x => x.DisplayName).MaximumLength(100).NotEmpty().WithName(x => localizer["Display Name"]);
            RuleFor(x => x.PluralDisplayName).MaximumLength(100).NotEmpty().WithName(x => localizer["Plural Display Name"]);
            RuleFor(x => x.BaseEntityId);
            RuleFor(x => x.IsAbstract);
            RuleForEach(x => x.Fields).SetValidator(new UpdateFieldValidator(localizer));
        }

    }
}
