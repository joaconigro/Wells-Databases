using System.Collections.Generic;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Wells.Base;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    public class BaseFilter<T> : IBaseFilter<T>
    {
        protected bool isEnabled;
        public virtual bool IsEditable => true;
        public string DisplayPropertyName { get; set; }
        public virtual string DisplayValue { get; }

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

        public virtual bool IsDateRangeFilter { get; }

        [XmlIgnore]
        public FilterCollection<T> ParentCollection { get; set; }

        public virtual IEnumerable<T> Apply(IEnumerable<T> queryable) { return null; }

        public virtual void SetUpdatedValues(FilterViewModel filterViewModel) { }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public virtual void ReadXml(XmlReader reader)
        {
            reader.Read();
            PropertyName = reader.ReadElementContentAsString();
            DisplayPropertyName = reader.ReadElementContentAsString();
            IsEnabled = reader.ReadElementContentAsBoolean();
        }
        public virtual void WriteXml(XmlWriter writer)
        {
            writer.Write(nameof(PropertyName), PropertyName);
            writer.Write(nameof(DisplayPropertyName), DisplayPropertyName);
            writer.Write(nameof(IsEnabled), IsEnabled);
        }

        public BaseFilter() { }

        protected BaseFilter(string name, string displayName)
        {
            PropertyName = name;
            DisplayPropertyName = displayName;
            IsEnabled = true;
        }
    }
}
