using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class CreateSolutionValidator : AbstractValidator<CreateSolution>
    {
        public CreateSolutionValidator()
        {
            RuleFor(x => x.Name).Length(50).NotEmpty();
            RuleFor(x => x.Path).Length(200).NotEmpty();
        }

    }
}
