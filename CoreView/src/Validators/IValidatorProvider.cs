using FluentValidation.Results;

namespace Wells.CoreView.Validators
{
    public interface IValidatorProvider
    {
        ValidationResult ValidationResult(string propertyName);
        bool IsValid(string propertyName);
    }
}
