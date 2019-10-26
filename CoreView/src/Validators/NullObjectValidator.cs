using FluentValidation;

namespace Wells.CoreView.Validators
{
    public class NullObjectValidator : AbstractValidator<object>
    {
        public NullObjectValidator(string propertyDisplayName)
        {
            RuleFor((o) => o).Must((o) => o != null).WithMessage($"{propertyDisplayName}: no puede ser nulo.");
        }
    }
}
