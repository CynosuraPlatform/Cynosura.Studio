﻿using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Files
{
    public class UpdateFileValidator : AbstractValidator<UpdateFile>
    {
        public UpdateFileValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty().WithName(x => localizer["Name"]);
            RuleFor(x => x.ContentType).MaximumLength(200).NotEmpty().WithName(x => localizer["Content Type"]);
            RuleFor(x => x.Content).NotEmpty().WithName(x => localizer["Content"]);
        }

    }
}
