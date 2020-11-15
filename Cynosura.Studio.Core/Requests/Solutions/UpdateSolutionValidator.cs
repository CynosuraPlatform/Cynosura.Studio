using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class UpdateSolutionValidator : AbstractValidator<UpdateSolution>
    {
        public UpdateSolutionValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Name).MaximumLength(50).NotEmpty().WithName(x => localizer["Name"]);
            RuleFor(x => x.Path).MaximumLength(200).NotEmpty().WithName(x => localizer["Path"]);
        }

    }
}
