﻿using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Enums
{
    public class UpdateEnumValidator : AbstractValidator<UpdateEnum>
    {
        public UpdateEnumValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty().Matches(RegexHelper.CSharpName).WithName(x => localizer["Name"]);
            RuleFor(x => x.DisplayName).MaximumLength(100).NotEmpty().WithName(x => localizer["Display Name"]);
        }

    }
}
