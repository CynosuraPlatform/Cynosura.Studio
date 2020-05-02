using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class CreateViewValidator : AbstractValidator<CreateView>
    {
        public CreateViewValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty();
        }

    }
}
