using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Wells.Base;
using Wells.Model;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    class WellFilter<T> : BaseFilter<T>
    {
        string WellName { get; set; }
        bool ApplyToWellsOnly { get; set; }
        public override string DisplayValue => WellName;
        public override bool IsEditable => false;
        public override bool IsDateRangeFilter => false;
        public override string Description => $"Pozo {WellName}";


        public WellFilter() { }

        public WellFilter(bool applyToWellsOnly, string wellName) :
            base("Well", string.Empty)
        {
            WellName = wellName;
            ApplyToWellsOnly = applyToWellsOnly;
        }

        public override void ReadXml(XmlReader reader)
        {
            base.ReadXml(reader);
            WellName = reader.ReadElementContentAsString();
            ApplyToWellsOnly = reader.ReadElementContentAsBoolean();
            reader.ReadEndElement();
        }

        public override void WriteXml(XmlWriter writer)
        {
            base.WriteXml(writer);
            writer.Write(nameof(WellName), WellName);
            writer.Write(nameof(ApplyToWellsOnly), ApplyToWellsOnly);
        }

        public override IEnumerable<T> Apply(IEnumerable<T> queryable)
        {
            if (ApplyToWellsOnly)
            {
                return from o in queryable
                       let w = o as object as Well
                       where w.Name == WellName
                       select o;
            }
            else
            {
                return from o in queryable
                       let w = (Well)Interaction.CallByName(o, PropertyName, CallType.Get)
                       where w.Name == WellName
                       select o;
            }
        }

        public override void SetUpdatedValues(FilterViewModel filterViewModel)
        {
            //Nothing to do.
        }

    }

}
