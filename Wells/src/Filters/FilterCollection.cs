using System;
using System.Collections.Generic;
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

        public IEnumerable<T> Apply(IEnumerable<T> queryable)
        {
            foreach (var f in this)
            {
                if (f.IsEnabled)
                {
                    queryable = f.Apply(queryable);
                }
            }
            return queryable;
        }
    }
}
