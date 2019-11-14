using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Wells.Base;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    public class EnumFilter<T> : BaseFilter<T>
    {
        public int Value { get; set; }
        public Type EnumType { get; private set; }
        public override string DisplayValue => Common.GetEnumDescription(EnumType, Value);

        public override bool IsDateRangeFilter => false;

        public override string Description
        {
            get
            {
                return $"{DisplayPropertyName} = {DisplayValue}";
            }
        }

        public EnumFilter() { }

        public EnumFilter(string propertyName, string displayName, int value, Type enumType) :
            base(propertyName, displayName)
        {
            Value = value;
            EnumType = enumType;
        }

        public override IEnumerable<T> Apply(IEnumerable<T> queryable)
        {
            return from o in queryable
                   where (int)Interaction.CallByName(o, PropertyName, CallType.Get) == Value
                   select o;
        }

        public override void SetUpdatedValues(FilterViewModel filterViewModel)
        {
            Value = filterViewModel.SelectedEnumValue;
        }

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);
            Value = reader.ReadElementContentAsInt();
            EnumType = reader.ReadContentAsType();
            reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.Write(nameof(Value), Value);
            writer.Write(nameof(EnumType), EnumType);
        }
    }
}
