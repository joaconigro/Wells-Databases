using System;
using Wells.BaseView.Validators;
using Wells.Persistence.Repositories;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    public class FilterFactory
    {
        public static BaseFilter<T> CreateFilter<T>(FilterViewModel vm)
        {
            var repo = RepositoryWrapper.Instance.Repository<T>();
            BaseFilter<T> f = null;

            if (vm.ShowNumericPanel)
            {
                f = CreateNumericFilter<T>(vm.PropertyName, vm.PropertyDisplayName, repo, vm.NumericValue, (NumericFunctions)vm.SelectedMathFunction);
            }
            else if (vm.ShowStringPanel)
            {
                f = CreateStringFilter<T>(vm.PropertyName, vm.PropertyDisplayName, repo, vm.StringValue);
            }
            else if (vm.ShowDatePanel)
            {
                f = CreateDateRangeFilter<T>(vm.PropertyName, vm.PropertyDisplayName, repo, vm.StartDate, vm.EndDate);
            }
            else if (vm.ShowBooleanPanel)
            {
                f = CreateBooleanFilter<T>(vm.PropertyName, vm.PropertyDisplayName, repo, vm.BooleanValue);
            }
            else if (vm.ShowEnumPanel)
            {
                f = CreateEnumFilter<T>(vm.PropertyName, vm.PropertyDisplayName, repo, vm.SelectedEnumValue, vm.FilterType);
            }

            return f;
        }


        private static StringFilter<T> CreateStringFilter<T>(string propertyName, string displayName, IBussinessObjectRepository repository, string value)
        {
            return new StringFilter<T>(propertyName, displayName, repository, value);
        }

        private static BooleanFilter<T> CreateBooleanFilter<T>(string propertyName, string displayName, IBussinessObjectRepository repository, bool value)
        {
            return new BooleanFilter<T>(propertyName, displayName, repository, value);
        }

        private static EnumFilter<T> CreateEnumFilter<T>(string propertyName, string displayName, IBussinessObjectRepository repository, int value, Type enumType)
        {
            return new EnumFilter<T>(propertyName, displayName, repository, value, enumType);
        }

        private static NumericFilter<T> CreateNumericFilter<T>(string propertyName, string displayName, IBussinessObjectRepository repository, double value, NumericFunctions function)
        {
            return new NumericFilter<T>(propertyName, displayName, repository, value, function);
        }

        private static DateRangeFilter<T> CreateDateRangeFilter<T>(string propertyName, string displayName, IBussinessObjectRepository repository, DateTime startDate)
        {
            return new DateRangeFilter<T>(propertyName, displayName, repository, startDate);
        }

        private static DateRangeFilter<T> CreateDateRangeFilter<T>(string propertyName, string displayName, IBussinessObjectRepository repository, DateTime startDate, DateTime endDate)
        {
            return new DateRangeFilter<T>(propertyName, displayName, repository, startDate, endDate);
        }
    }
}
