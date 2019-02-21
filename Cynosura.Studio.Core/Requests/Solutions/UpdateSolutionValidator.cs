using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class UpdateSolutionValidator : AbstractValidator<UpdateSolution>
    {
        public UpdateSolutionValidator()
        {
            RuleFor(x => x.Name).MaximumLength(50).NotEmpty().WithName("Name");
            RuleFor(x => x.Path).MaximumLength(200).NotEmpty().WithName("Path");
        }

    }
}
