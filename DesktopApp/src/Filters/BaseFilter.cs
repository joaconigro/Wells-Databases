using System.Linq;
using Wells.CorePersistence.Repositories;

namespace Wells.View.Filters
{
    public abstract class BaseFilter<T> : IBaseFilter
    {
        protected IBussinessObjectRepository repository;
        protected bool isEnabled;
        protected bool isEditable;

        public bool IsEditable => isEditable;
        public string DisplayPropertyName { get; set; }

        public abstract string DisplayValue { get; }

        public virtual string Description
        {
            get
            {
                return DisplayPropertyName;
            }
        }

        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                ParentCollection?.RaiseCollectionChanged();
            }
        }
        public string PropertyName { get; set; }

        public abstract bool IsDateRangeFilter { get; }

        public FilterCollection<T> ParentCollection { get; set; }

        public abstract IQueryable<T> Apply(IQueryable<T> queryable);

        protected BaseFilter(string name, string displayName, IBussinessObjectRepository repo)
        {
            PropertyName = name;
            DisplayPropertyName = displayName;
            repository = repo;
            IsEnabled = true;
            isEditable = true;
        }
    }
}
