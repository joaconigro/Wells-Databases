using FluentValidation;

namespace Wells.BaseView.Validators
{
    public class EmptyStringValidator : AbstractValidator<string>
    {
        public EmptyStringValidator(string propertyDisplayName)
        {
            RuleFor((text) => text).Must((text) => !string.IsNullOrEmpty(text)).WithMessage($"{propertyDisplayName}: no puede quedar vacío.");
        }
    }
}
