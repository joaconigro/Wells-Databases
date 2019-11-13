using System;
using Wells.BaseView.Validators;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    public static class FilterFactory
    {
        public static IBaseFilter<T> CreateFilter<T>(FilterViewModel vm)
        {
            if (vm.ShowNumericPanel)
            {
                return CreateNumericFilter<T>(vm.PropertyName, vm.PropertyDisplayName, vm.NumericValue, (NumericFunctions)vm.SelectedMathFunction);
            }
            else if (vm.ShowStringPanel)
            {
                return CreateStringFilter<T>(vm.PropertyName, vm.PropertyDisplayName, vm.StringValue);
            }
            else if (vm.ShowDatePanel)
            {
                return CreateDateRangeFilter<T>(vm.PropertyName, vm.PropertyDisplayName, vm.StartDate, vm.EndDate);
            }
            else if (vm.ShowBooleanPanel)
            {
                return CreateBooleanFilter<T>(vm.PropertyName, vm.PropertyDisplayName, vm.BooleanValue);
            }
            else if (vm.ShowEnumPanel)
            {
                return CreateEnumFilter<T>(vm.PropertyName, vm.PropertyDisplayName, vm.SelectedEnumValue, vm.FilterType);
            }
            return null;
        }

        public static IBaseFilter<T> CreateFilter<T>(string className)
        {
            return className switch
            {
                "NumericFilter" => new NumericFilter<T>(),
                "BooleanFilter" => new BooleanFilter<T>(),
                "StringFilter" => new StringFilter<T>(),
                "DateRangeFilter" => new DateRangeFilter<T>(),
                "EnumFilter" => new EnumFilter<T>(),
                "WellFilter" => new WellFilter<T>(),
                _ => null
            };
        }


        private static StringFilter<T> CreateStringFilter<T>(string propertyName, string displayName, string value)
        {
            return new StringFilter<T>(propertyName, displayName, value);
        }

        private static BooleanFilter<T> CreateBooleanFilter<T>(string propertyName, string displayName, bool value)
        {
            return new BooleanFilter<T>(propertyName, displayName, value);
        }

        private static EnumFilter<T> CreateEnumFilter<T>(string propertyName, string displayName, int value, Type enumType)
        {
            return new EnumFilter<T>(propertyName, displayName, value, enumType);
        }

        private static NumericFilter<T> CreateNumericFilter<T>(string propertyName, string displayName, double value, NumericFunctions function)
        {
            return new NumericFilter<T>(propertyName, displayName, value, function);
        }

        private static DateRangeFilter<T> CreateDateRangeFilter<T>(string propertyName, string displayName, DateTime startDate, DateTime endDate)
        {
            return new DateRangeFilter<T>(propertyName, displayName, startDate, endDate);
        }
    }
}
