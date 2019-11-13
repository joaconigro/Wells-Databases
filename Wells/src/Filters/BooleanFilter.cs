using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Wells.Base;
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

        public BooleanFilter() { }

        public BooleanFilter(string propertyName, string displayName, bool value) :
            base(propertyName, displayName)
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

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);
            Value = reader.ReadElementContentAsBoolean();
            reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.Write(nameof(Value), Value);
        }
    }
}
