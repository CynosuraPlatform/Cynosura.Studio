using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class CreateSolutionValidator : AbstractValidator<CreateSolution>
    {
        public CreateSolutionValidator()
        {
            RuleFor(x => x.Name).MaximumLength(50).NotEmpty();
            RuleFor(x => x.Path).MaximumLength(200).NotEmpty();
        }

    }
}
