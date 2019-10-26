﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Wells.CorePersistence.Repositories;

namespace Wells.CoreView.Validators
{
    public class WellNameValidator : AbstractValidator<string>
    {
        public WellNameValidator(string propertyDisplayName)
        {
            RuleFor((text) => text).Must((text) => !RepositoryWrapper.Instance.Wells.ContainsName(text)).WithMessage($"{propertyDisplayName}: existe otro pozo con el mismo nombre.");
        }
    }
}
