using System.ComponentModel;
using FluentValidation;

namespace Wells.BaseView.Validators
{
    public class NumberValidator<T> : AbstractValidator<T>
    {
        readonly NumericFunctions _FuntionType;
        readonly double _LimitValue;
        readonly NumericFunctions _LowerFunctionRange;
        readonly NumericFunctions _UpperFunctionRange;
        readonly double _LowerValue;
        readonly double _UpperValue;

        public NumberValidator(string propertyDisplayName, NumericFunctions functionType, T limitValue)
        {
            _FuntionType = functionType;
            _LimitValue = System.Convert.ToDouble(limitValue);

            RuleFor((number) => number).Must((number) => CompareValue(number)).WithMessage($"{propertyDisplayName}: el valor no es correcto.");
        }

        public NumberValidator(string propertyDisplayName, T lowerLimit, bool inclusiveLowerLimit, T upperLimit, bool inclusiveUpperLimit)
        {
            _LowerFunctionRange = inclusiveLowerLimit ? NumericFunctions.GreaterOrEqual : NumericFunctions.Greater;
            _UpperFunctionRange = inclusiveUpperLimit ? NumericFunctions.LowerOrEqual : NumericFunctions.Lower;
            _LowerValue = System.Convert.ToDouble(lowerLimit);
            _UpperValue = System.Convert.ToDouble(upperLimit);   

            RuleFor((number) => number).Must((number) => CompareRange(number)).WithMessage($"{propertyDisplayName}: el valor no está en el rango esperado.");
        }


        bool CompareRange(T aValue)
        {
            double doubleValue = System.Convert.ToDouble(aValue);

            bool lowerResult;
            if (_LowerFunctionRange == NumericFunctions.Greater)
            {
                lowerResult = doubleValue > _LowerValue;
            }
            else
            {
                lowerResult = doubleValue >= _LowerValue;
            }

            bool upperResult;
            if (_UpperFunctionRange == NumericFunctions.Lower)
            {
                upperResult = doubleValue < _UpperValue;
            }
            else
            {
                upperResult = doubleValue <= _UpperValue;
            }
            return lowerResult && upperResult;
        }

        bool CompareValue(T aValue)
        {
            double doubleValue = System.Convert.ToDouble(aValue);
            switch (_FuntionType)
            {
                case NumericFunctions.Equal:
                    return _LimitValue.Equals(System.Math.Round(doubleValue));
                case NumericFunctions.Greater:
                    return doubleValue > _LimitValue;
                case NumericFunctions.GreaterOrEqual:
                    return doubleValue >= _LimitValue;
                case NumericFunctions.Lower:
                    return doubleValue < _LimitValue;
                case NumericFunctions.LowerOrEqual:
                    return doubleValue <= _LimitValue;
                default:
                    return false;
            }
        }
    }

    public enum NumericFunctions
    {
        [Description("Igual")] Equal,
        [Description("Menor")] Lower,
        [Description("Menor o igual")] LowerOrEqual,
        [Description("Mayor")] Greater,
        [Description("Mayor o igual")] GreaterOrEqual
    }
}
