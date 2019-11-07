using Microsoft.VisualBasic;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Wells.Model;
using Wells.Persistence.Repositories;
using Wells.View.ViewModels;

namespace Wells.View.Filters
{
    class WellFilter<T> : BaseFilter<T>
    {
        WellQueryProperty QueryProperty { get; }
        WellTypes WellType { get; }
        string WellName { get; }

        readonly bool _ApplyToWellsOnly;
        public override string DisplayValue => WellName;

        public override bool IsDateRangeFilter => false;

        public override string Description => CreateDescription();




        public WellFilter(IBussinessObjectRepository repo, bool applyToWellsOnly, int wellType, int wellProperty, string wellName) :
            base("Well", string.Empty, repo)
        {
            isEditable = false;
            WellType = (WellTypes)wellType;
            QueryProperty = (WellQueryProperty)wellProperty;
            WellName = wellName;
            _ApplyToWellsOnly = applyToWellsOnly;
        }


        public override IEnumerable<T> Apply(IEnumerable<T> queryable)
        {
            if (_ApplyToWellsOnly)
            {
                return from o in queryable
                       let w = ((o as object) as Well)
                       where IsThisWell(w)
                       select o;
            }
            else
            {
                return from o in queryable
                       let w = (Well)(Interaction.CallByName(o, PropertyName, CallType.Get))
                       where IsThisWell(w)
                       select o;
            }
        }

        bool IsThisWell(Well well)
        {
            var result = FilterByWellType(well);
            if (!result) { return false; }
            return FilterByWellProperty(well);
        }

        bool FilterByWellType(Well well)
        {
            return WellType switch
            {
                WellTypes.OnlyMeasurementWell => well.WellType == BaseModel.Models.WellType.MeasurementWell,
                WellTypes.OnlySounding => well.WellType == BaseModel.Models.WellType.Sounding,
                _ => true,
            };
        }

        public override void SetUpdatedValues(FilterViewModel filterViewModel)
        {
            //Nothing to do.
        }

        bool FilterByWellProperty(Well well)
        {
            switch (QueryProperty)
            {
                case WellQueryProperty.Name:
                    if (string.IsNullOrEmpty(WellName))
                    {
                        return false;
                    }
                    else
                    {
                        return well.Name == WellName;
                    }

                case WellQueryProperty.ZoneA:
                    return Rectangle2D.ZoneA.Contains(well);
                case WellQueryProperty.ZoneB:
                    return Rectangle2D.ZoneB.Contains(well);
                case WellQueryProperty.ZoneC:
                    return Rectangle2D.ZoneC.Contains(well);
                case WellQueryProperty.ZoneD:
                    return Rectangle2D.ZoneD.Contains(well);
                case WellQueryProperty.Torches:
                    return Rectangle2D.Torches.Contains(well);
                default:
                    return true;
            }
        }

        string CreateDescription()
        {
            var text = WellType switch
            {
                WellTypes.OnlyMeasurementWell => "Todos los pozos ",
                WellTypes.OnlySounding => "Todos los sondeos ",
                _ => "Todos los pozos y sondeos ",
            };
            switch (QueryProperty)
            {
                case WellQueryProperty.Name:
                    text += $"con nombre = {WellName}";
                    break;
                case WellQueryProperty.ZoneA:
                    text += "dentro de la zona A";
                    break;
                case WellQueryProperty.ZoneB:
                    text += "dentro de la zona B";
                    break;
                case WellQueryProperty.ZoneC:
                    text += "dentro de la zona C";
                    break;
                case WellQueryProperty.ZoneD:
                    text += "dentro de la zona D";
                    break;
                case WellQueryProperty.Torches:
                    text += "dentro de la zona Antorchas";
                    break;
                default:
                    text = text.Trim();
                    break;
            }

            return text;
        }
    }


    public enum WellTypes
    {
        [Description("Todos")] All,
        [Description("Pozo")] OnlyMeasurementWell,
        [Description("Sondeo")] OnlySounding
    }

    public enum WellQueryProperty
    {
        [Description("Ninguna")] None,
        [Description("Nombre")] Name,
        [Description("Zona A")] ZoneA,
        [Description("Zona B")] ZoneB,
        [Description("Zona C")] ZoneC,
        [Description("Zona D")] ZoneD,
        [Description("Zona Antorchas")] Torches
    }
}
