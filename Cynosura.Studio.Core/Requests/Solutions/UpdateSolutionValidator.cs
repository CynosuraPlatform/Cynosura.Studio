using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Solutions
{
    public class UpdateSolutionValidator : AbstractValidator<CreateSolution>
    {
        public UpdateSolutionValidator()
        {
            RuleFor(x => x.Name).Length(50).NotEmpty();
            RuleFor(x => x.Path).Length(200).NotEmpty();
        }

    }
}
