using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class CreateSolutionValidator : AbstractValidator<CreateSolution>
    {
        public CreateSolutionValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Name).MaximumLength(50).NotEmpty().WithName(x => localizer["Name"]);
            RuleFor(x => x.Path).MaximumLength(200).NotEmpty().WithName(x => localizer["Path"]);
            RuleFor(x => x.TemplateName).NotEmpty().WithName(x => localizer["Template Name"]);
        }

    }
}
