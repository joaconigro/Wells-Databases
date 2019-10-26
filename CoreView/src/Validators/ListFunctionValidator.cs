using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Wells.CoreView.Validators
{
    public class ListFunctionValidator<T> : AbstractValidator<IEnumerable<T>>
    {
        public ListFunctionValidator(string propertyDisplayName, Func<T, bool> predicate)
        {
            RuleFor((list) => list).Must((list) => list != null && list.All(predicate)).WithMessage($"{propertyDisplayName}: al menos un elemento no cumple los requisitos.");
        }
    }
}
