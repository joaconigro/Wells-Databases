namespace Wells.View.Filters
{
    public interface IBaseFilter
    {
        string DisplayPropertyName { get; set; }
        string DisplayValue { get; }
        string Description { get; }
        bool IsEnabled { get; set; }
        string PropertyName { get; set; }
        bool IsDateRangeFilter { get; }
    }
}
