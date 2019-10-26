using System;
using System.Collections.ObjectModel;
using System.Linq;


namespace Wells.View.Filters
{
    public class FilterCollection<T> : ObservableCollection<BaseFilter<T>>
    {

        protected string PropertyName;

        public EventHandler FiltersChanged { get; set; }
        public void RaiseCollectionChanged()
        {
            FiltersChanged(this, EventArgs.Empty);
        }

        public IQueryable<T> Apply(IQueryable<T> queryable)
        {
            foreach (var f in this)
            {
                if (f.IsEnabled)
                    queryable = f.Apply(queryable);
            }
            return queryable;
        }
    }
}
