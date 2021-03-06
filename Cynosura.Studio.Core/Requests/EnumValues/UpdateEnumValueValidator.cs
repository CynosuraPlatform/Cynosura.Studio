﻿using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.EnumValues
{
    public class UpdateEnumValueValidator : AbstractValidator<UpdateEnumValue>
    {
        public UpdateEnumValueValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty().Matches(RegexHelper.CSharpName).WithName(x => localizer["Name"]);
            RuleFor(x => x.DisplayName).MaximumLength(100).WithName(x => localizer["Display Name"]);
            RuleFor(x => x.Value);
        }

    }
}
