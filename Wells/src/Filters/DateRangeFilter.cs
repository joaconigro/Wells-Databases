using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Wells.Base;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    public class DateRangeFilter<T> : BaseFilter<T>
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public override string DisplayValue => $"Entre {StartDate.ToString("dd/MM/yy")} y {EndDate.ToString("dd/MM/yy")}";

        public override bool IsDateRangeFilter => true;

        public DateRangeFilter(string propertyName, string displayName, DateTime startDate) :
            base(propertyName, displayName)
        {
            StartDate = startDate;
            EndDate = DateTime.Today;
        }

        public DateRangeFilter() { }

        public DateRangeFilter(string propertyName, string displayName, DateTime startDate, DateTime endDate) :
           base(propertyName, displayName)
        {
            StartDate = startDate;
            EndDate = endDate;
        }

        public override IEnumerable<T> Apply(IEnumerable<T> queryable)
        {
            return from o in queryable
                   where CompareValue((DateTime)Interaction.CallByName(o, PropertyName, CallType.Get))
                   select o;
        }

        bool CompareValue(DateTime objectValue)
        {
            return objectValue >= StartDate && objectValue <= EndDate;
        }

        public override void SetUpdatedValues(FilterViewModel filterViewModel)
        {
            StartDate = filterViewModel.StartDate;
            EndDate = filterViewModel.EndDate;
        }

        public override string Description => $"{DisplayPropertyName} entre {StartDate.ToString("dd/MM/yy")} y {EndDate.ToString("dd/MM/yy")}";

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);
            StartDate = reader.ReadContentAsDateTime();
            EndDate = reader.ReadContentAsDateTime();
            reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.Write(nameof(StartDate), StartDate);
            writer.Write(nameof(EndDate), EndDate);
        }
    }
}
