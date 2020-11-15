using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class CreateViewValidator : AbstractValidator<CreateView>
    {
        public CreateViewValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty().WithName(x => localizer["Name"]);
        }

    }
}
