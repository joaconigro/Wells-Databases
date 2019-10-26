using FluentValidation;
using System.Collections.Generic;
using System.Linq;

namespace Wells.CoreView.Validators
{
    public class EmptyListValidator<T> : AbstractValidator<IEnumerable<T>>
    {
        public EmptyListValidator(string propertyDisplayName)
        {
            RuleFor((list) => list).Must((list) => list != null && list.Any()).WithMessage($"{propertyDisplayName}: debe tener al menos un elemento.");
        }
    }
}
