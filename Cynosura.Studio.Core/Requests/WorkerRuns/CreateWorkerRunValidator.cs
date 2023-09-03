using FluentValidation;
using Microsoft.Extensions.Localization;
using Cynosura.Studio.Core.Infrastructure.Validators;

namespace Cynosura.Studio.Core.Requests.WorkerRuns
{
    public class CreateWorkerRunValidator : AbstractValidator<CreateWorkerRun>
    {
        public CreateWorkerRunValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(x => x.WorkerInfoId).NotEmpty().WithName(x => localizer["Worker"]);
            RuleFor(x => x.Data).IsJson().WithName(x => localizer["Data"]);
        }

    }
}
