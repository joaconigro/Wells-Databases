using System.Collections.Generic;
using System.Xml.Serialization;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    public interface IBaseFilter<T> : IXmlSerializable
    {
        string DisplayPropertyName { get; set; }
        string DisplayValue { get; }
        string Description { get; }
        bool IsEnabled { get; set; }
        string PropertyName { get; set; }
        bool IsDateRangeFilter { get; }
        FilterCollection<T> ParentCollection { get; set; }
        IEnumerable<T> Apply(IEnumerable<T> enumerable);
        void SetUpdatedValues(FilterViewModel filterViewModel);
    }
}
