using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Wells.BaseModel.Models;

namespace Wells.Model
{
    public class FLNAAnalysis : ChemicalAnalysis
    {
        public FLNAAnalysis() : base() { }

        public FLNAAnalysis(Well well) : base(well) { }

        public override ChemicalAnalysisType GetChemicalAnalysisType(string propertyName)
        {
            return FLNAAnalysisTypes[propertyName];
        }

        #region Properties
        [DisplayName("GRO (%)"), Browsable(true)]
        public double GRO { get; set; }

        [DisplayName("DRO (%)"), Browsable(true)]
        public double DRO { get; set; }

        [DisplayName("MRO (%)"), Browsable(true)]
        public double MRO { get; set; }

        [DisplayName("Benceno (%)"), Browsable(true)]
        public double Benzene { get; set; }

        [DisplayName("Tolueno (%)"), Browsable(true)]
        public double Tolueno { get; set; }

        [DisplayName("Etilbenceno (%)"), Browsable(true)]
        public double Ethylbenzene { get; set; }

        [DisplayName("Xilenos (%)"), Browsable(true)]
        public double Xylenes { get; set; }

        [DisplayName("C6 - C8 (%)"), Browsable(true)]
        public double C6_C8 { get; set; }

        [DisplayName("C8 - C10 (%)"), Browsable(true)]
        public double C8_C10 { get; set; }

        [DisplayName("C10 - C12 (%)"), Browsable(true)]
        public double C10_C12 { get; set; }

        [DisplayName("C12 - C16 (%)"), Browsable(true)]
        public double C12_C16 { get; set; }

        [DisplayName("C16 - C21 (%)"), Browsable(true)]
        public double C16_C21 { get; set; }

        [DisplayName("C21 - C35 (%)"), Browsable(true)]
        public double C21_C35 { get; set; }

        [DisplayName("C17/Pristano"), Browsable(true)]
        public double C17_Pristano { get; set; }

        [DisplayName("C18/Fitano"), Browsable(true)]
        public double C18_Fitano { get; set; }

        [DisplayName("Densidad Real (g/cm3)"), Browsable(true)]
        public double RealDensity { get; set; }

        [DisplayName("Viscosidad Dinámica (cP)"), Browsable(true)]
        public double DynamicViscosity { get; set; }
        #endregion

        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                return GetBrowsableProperties(typeof(FLNAAnalysis));
            }
        }

        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> DoubleProperties
        {
            get
            {
                return GetDoubleBrowsableProperties(typeof(FLNAAnalysis));
            }
        }

        public static string GetDisplayName(string propertyName)
        {
            return GetDisplayName(typeof(FLNAAnalysis), propertyName);
        }

        public static string GetChemicalAnalysisUnits(string propertyName)
        {
            return FLNAAnalysisTypes[propertyName].Unit;
        }
    }
}
