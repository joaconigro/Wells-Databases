using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Wells.Base;

namespace Wells.View.Filters
{
    public class FilterCollection<T> : ObservableCollection<IBaseFilter<T>>, IXmlSerializable
    {
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

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.MoveToContent();
            reader.Read();
            var list = reader.ReadElementContentAsList<IBaseFilter<T>>(FilterFactory.CreateFilter<T>);
            foreach (var f in list)
            {
                Add(f);
                f.ParentCollection = this;
            }
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.Write(nameof(Items), Items);
        }
    }
}
