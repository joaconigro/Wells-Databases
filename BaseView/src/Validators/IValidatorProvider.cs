using FluentValidation.Results;

namespace Wells.BaseView.Validators
{
    public interface IValidatorProvider
    {
        ValidationResult ValidationResult(string propertyName);
        bool IsValid(string propertyName);
    }
}
