using FluentValidation;
using Wells.Persistence.Repositories;

namespace Wells.BaseView.Validators
{
    public class WellNameValidator : AbstractValidator<string>
    {
        public WellNameValidator(string propertyDisplayName)
        {
            RuleFor((text) => text).Must((text) => !RepositoryWrapper.Instance.Wells.ContainsName(text.ToUpperInvariant())).WithMessage($"{propertyDisplayName}: existe otro pozo con el mismo nombre.");
        }
    }
}
