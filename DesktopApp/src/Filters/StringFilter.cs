using Microsoft.VisualBasic;
using System.Linq;
using Wells.CorePersistence.Repositories;

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

        public override IQueryable<T> Apply(IQueryable<T> queryable)
        {
            return from o in queryable
                   let s = (string)Interaction.CallByName(o, PropertyName, CallType.Get)
                   where !string.IsNullOrEmpty(s) && s.ToLower().Contains(Value.ToLower())
                   select o;
        }
    }
}
