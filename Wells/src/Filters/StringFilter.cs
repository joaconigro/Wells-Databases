using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using Wells.Persistence.Repositories;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    public class StringFilter<T> : BaseFilter<T>
    {
        public string Value { get; set; }
        public override string DisplayValue => Value;

        public override bool IsDateRangeFilter => false;

        public override string Description
        {
            get
            {
                return $"{DisplayPropertyName} contiene {Value}";
            }
        }

        public StringFilter(string propertyName, string displayName, IBussinessObjectRepository repo, string value) :
            base(propertyName, displayName, repo)
        {
            Value = value;
        }

        public override IEnumerable<T> Apply(IEnumerable<T> queryable)
        {
            return from o in queryable
                   let s = (string)Interaction.CallByName(o, PropertyName, CallType.Get)
                   where !string.IsNullOrEmpty(s) && s.ToLower().Contains(Value.ToLower())
                   select o;
        }

        public override void SetUpdatedValues(FilterViewModel filterViewModel)
        {
            Value = filterViewModel.StringValue;
        }
    }
}
