using FluentValidation;

namespace Cynosura.Studio.Core.Requests.Views
{
    public class UpdateViewValidator : AbstractValidator<UpdateView>
    {
        public UpdateViewValidator()
        {
            RuleFor(x => x.Name).MaximumLength(100).NotEmpty();
        }

    }
}
