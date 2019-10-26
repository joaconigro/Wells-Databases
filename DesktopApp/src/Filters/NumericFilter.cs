
using Microsoft.VisualBasic;
using System;
using System.Linq;
using Wells.CorePersistence.Repositories;
using Wells.CoreView.Validators;

namespace Wells.View.Filters
{
    public class NumericFilter<T> : BaseFilter<T>
    {
        public double Value { get; set; }

        public NumericFunctions Function { get; set; }
        public override string DisplayValue => Value.ToString("N2");

        public override bool IsDateRangeFilter => false;

        public NumericFilter(string propertyName, string displayName, IBussinessObjectRepository repo, double value, NumericFunctions function) :
            base(propertyName, displayName, repo)
        {
            Value = value;
            Function = function;
        }

        public override IQueryable<T> Apply(IQueryable<T> queryable)
        {
            return from o in queryable
                   where CompareValue((double)Interaction.CallByName(o, PropertyName, CallType.Get))
                   select o;
        }

        bool CompareValue(double objectValue)
        {
            switch (Function)
            {
                case NumericFunctions.Equal:
                    return objectValue == System.Math.Round(Value);
                case NumericFunctions.Greater:
                    return objectValue > Value;
                case NumericFunctions.GreaterOrEqual:
                    return objectValue >= Value;
                case NumericFunctions.Lower:
                    return objectValue < Value;
                case NumericFunctions.LowerOrEqual:
                    return objectValue <= Value;
                default:
                    return false;
            }
        }


        public override string Description
        {
            get
            {
                switch (Function)
                {
                    case NumericFunctions.Equal:
                        return $"{DisplayPropertyName} = {DisplayValue}";
                    case NumericFunctions.Greater:
                        return $"{DisplayPropertyName} > {DisplayValue}";
                    case NumericFunctions.GreaterOrEqual:
                        return $"{DisplayPropertyName} >= {DisplayValue}";
                    case NumericFunctions.Lower:
                        return $"{DisplayPropertyName} < {DisplayValue}";
                    default:
                        return $"{DisplayPropertyName} <= {DisplayValue}";
                }
            }
        }
    }
}
