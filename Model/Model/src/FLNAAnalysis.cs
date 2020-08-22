using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Wells.BaseModel.Attributes;

namespace Wells.Model
{
    public class FlnaAnalysis : ChemicalAnalysis
    {
        public FlnaAnalysis() { }

        public FlnaAnalysis(Well well) : base(well) { }

        public override ChemicalAnalysisType GetChemicalAnalysisType(string propertyName)
        {
            return FlnaAnalysisTypes[propertyName.ToLowerInvariant()];
        }

        #region Properties
        [DisplayName("DRO (%)"), Browsable(true), NullValue()]
        public double Dro { get; set; }

        [DisplayName("GRO (%)"), Browsable(true), NullValue()]
        public double Gro { get; set; }

        [DisplayName("MRO (%)"), Browsable(true), NullValue()]
        public double Mro { get; set; }

        [DisplayName("Benceno (%)"), Browsable(true), NullValue()]
        public double Benzene { get; set; }

        [DisplayName("Tolueno (%)"), Browsable(true), NullValue()]
        public double Tolueno { get; set; }

        [DisplayName("Etilbenceno (%)"), Browsable(true), NullValue()]
        public double Ethylbenzene { get; set; }

        [DisplayName("Xilenos (%)"), Browsable(true), NullValue()]
        public double Xylenes { get; set; }

        [DisplayName("C6 - C8 (%)"), Browsable(true), NullValue()]
        public double C6_C8 { get; set; }

        [DisplayName("C8 - C10 (%)"), Browsable(true), NullValue()]
        public double C8_C10 { get; set; }

        [DisplayName("C10 - C12 (%)"), Browsable(true), NullValue()]
        public double C10_C12 { get; set; }

        [DisplayName("C12 - C16 (%)"), Browsable(true), NullValue()]
        public double C12_C16 { get; set; }

        [DisplayName("C16 - C21 (%)"), Browsable(true), NullValue()]
        public double C16_C21 { get; set; }

        [DisplayName("C21 - C35 (%)"), Browsable(true), NullValue()]
        public double C21_C35 { get; set; }

        [DisplayName("C17/Pristano"), Browsable(true), NullValue()]
        public double C17_Pristano { get; set; }

        [DisplayName("C18/Fitano"), Browsable(true), NullValue()]
        public double C18_Fitano { get; set; }

        [DisplayName("Densidad Real (g/cm3)"), Browsable(true), NullValue()]
        public double RealDensity { get; set; }

        [DisplayName("Viscosidad Dinámica (cP)"), Browsable(true), NullValue()]
        public double DynamicViscosity { get; set; }
        #endregion

        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                return GetBrowsableProperties(typeof(FlnaAnalysis));
            }
        }

        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> DoubleProperties
        {
            get
            {
                return GetDoubleBrowsableProperties(typeof(FlnaAnalysis));
            }
        }

        public static string GetDisplayName(string propertyName)
        {
            return GetDisplayName(typeof(FlnaAnalysis), propertyName);
        }

        public static string GetChemicalAnalysisUnits(string propertyName)
        {
            return FlnaAnalysisTypes[propertyName].Unit;
        }
    }
}
