using System;
using System.Collections.Generic;
using System.Text;
using Wells.Persistence.Repositories;
using Wells.BaseView.Validators;

namespace Wells.View.Filters
{
    public class FilterFactory
    {
        private FilterFactory() { }

        public static StringFilter<T> CreateStringFilter<T>(string propertyName, string displayName, IBussinessObjectRepository repository, string value)
        {
            return new StringFilter<T>(propertyName, displayName, repository, value);
        }

        public static BooleanFilter<T> CreateBooleanFilter<T>(string propertyName, string displayName, IBussinessObjectRepository repository, bool value)
        {
            return new BooleanFilter<T>(propertyName, displayName, repository, value);
        }

        public static EnumFilter<T> CreateEnumFilter<T>(string propertyName, string displayName, IBussinessObjectRepository repository, int value, Type enumType)
        {
            return new EnumFilter<T>(propertyName, displayName, repository, value, enumType);
        }

        public static NumericFilter<T> CreateNumericFilter<T>(string propertyName, string displayName, IBussinessObjectRepository repository, double value, NumericFunctions function)
        {
            return new NumericFilter<T>(propertyName, displayName, repository, value, function);
        }

        public static DateRangeFilter<T> CreateDateRangeFilter<T>(string propertyName, string displayName, IBussinessObjectRepository repository, DateTime startDate)
        {
            return new DateRangeFilter<T>(propertyName, displayName, repository, startDate);
        }

        public static DateRangeFilter<T> CreateDateRangeFilter<T>(string propertyName, string displayName, IBussinessObjectRepository repository, DateTime startDate, DateTime endDate)
        {
            return new DateRangeFilter<T>(propertyName, displayName, repository, startDate, endDate);
        }
    }
}
