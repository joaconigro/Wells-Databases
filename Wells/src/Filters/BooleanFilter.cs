using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using Wells.Persistence.Repositories;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    public class BooleanFilter<T> : BaseFilter<T>
    {
        public bool Value { get; set; }
        public override string DisplayValue => Value ? "Verdadero" : "Falso";

        public override bool IsDateRangeFilter => false;

        public override string Description
        {
            get
            {
                return $"{DisplayPropertyName} = {DisplayValue}";
            }
        }

        public BooleanFilter(string propertyName, string displayName, IBussinessObjectRepository repo, bool value) :
            base(propertyName, displayName, repo)
        {
            Value = value;
        }

        public override IEnumerable<T> Apply(IEnumerable<T> queryable)
        {
            return from o in queryable
                   where (bool)(Interaction.CallByName(o, PropertyName, CallType.Get)) == Value
                   select o;
        }

        public override void SetUpdatedValues(FilterViewModel filterViewModel)
        {
            Value = filterViewModel.BooleanValue;
        }
    }
}
