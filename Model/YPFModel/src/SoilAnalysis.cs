using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Wells.StandardModel.Models;

namespace Wells.YPFModel
{
    public class SoilAnalysis : ChemicalAnalysis
    {
        public SoilAnalysis() : base() { }

        public SoilAnalysis(Well well) : base(well) { }

        public override ChemicalAnalysisType GetChemicalAnalysisType(string propertyName)
        {
            return SoilAnalysisTypes[propertyName];
        }


        #region Properties
        [DisplayName("Humedad (%)"), Browsable(true)]
        public double Humidity { get; set; }

        [DisplayName("pH (mg/Kg)"), Browsable(true)]
        public double PH { get; set; }

        [DisplayName("GRO (mg/Kg)"), Browsable(true)]
        public double GRO { get; set; }

        [DisplayName("DRO (mg/Kg)"), Browsable(true)]
        public double DRO { get; set; }

        [DisplayName("MRO (mg/Kg)"), Browsable(true)]
        public double MRO { get; set; }

        [DisplayName("Hidrocarburos totales (EPA 8015) (mg/Kg)"), Browsable(true)]
        public double TotalHydrocarbons_EPA8015 { get; set; }

        [DisplayName("Hidrocarburos totales (TNRCC 1005) (mg/Kg)"), Browsable(true)]
        public double TotalHydrocarbons_TNRCC1005 { get; set; }

        [DisplayName("Aceites y grasas (mg/Kg)"), Browsable(true)]
        public double OilsAndFats { get; set; }

        [DisplayName("> C6 - C8 (F. alifática) (mg/Kg)"), Browsable(true)]
        public double C6_C8Aliphatic { get; set; }

        [DisplayName("> C8 - C10 (F. alifática) (mg/Kg)"), Browsable(true)]
        public double C8_C10Aliphatic { get; set; }

        [DisplayName("> C10 - C12 (F. alifática) (mg/Kg)"), Browsable(true)]
        public double C10_C12Aliphatic { get; set; }

        [DisplayName("> C12 - C16 (F. alifática) (mg/Kg)"), Browsable(true)]
        public double C12_C16Aliphatic { get; set; }

        [DisplayName("> C16 - C21 (F. alifática) (mg/Kg)"), Browsable(true)]
        public double C16_C21Aliphatic { get; set; }

        [DisplayName("> C21 - C35 (F. alifática) (mg/Kg)"), Browsable(true)]
        public double C21_C35Aliphatic { get; set; }

        [DisplayName("> C6 - C8 (F. aromática) (mg/Kg)"), Browsable(true)]
        public double C6_C8Aromatic { get; set; }

        [DisplayName("> C8 - C10 (F. aromática) (mg/Kg)"), Browsable(true)]
        public double C8_C10Aromatic { get; set; }

        [DisplayName("> C10 - C12 (F. aromática) (mg/Kg)"), Browsable(true)]
        public double C10_C12Aromatic { get; set; }

        [DisplayName("> C12 - C16 (F. aromática) (mg/Kg)"), Browsable(true)]
        public double C12_C16Aromatic { get; set; }

        [DisplayName("> C16 - C21 (F. aromática) (mg/Kg)"), Browsable(true)]
        public double C16_C21Aromatic { get; set; }

        [DisplayName("> C21 - C35 (F. aromática) (mg/Kg)"), Browsable(true)]
        public double C21_C35Aromatic { get; set; }

        [DisplayName("Benceno (mg/Kg)"), Browsable(true)]
        public double Benzene { get; set; }

        [DisplayName("Tolueno (mg/Kg)"), Browsable(true)]
        public double Tolueno { get; set; }

        [DisplayName("Etilbenceno (mg/Kg)"), Browsable(true)]
        public double Ethylbenzene { get; set; }

        [DisplayName("Xileno (o) (mg/l)"), Browsable(true)]
        public double XyleneO { get; set; }

        [DisplayName("Xileno (p-m) (mg/l)"), Browsable(true)]
        public double XylenePM { get; set; }

        [DisplayName("Xileno total (mg/l)"), Browsable(true)]
        public double TotalXylene { get; set; }

        [DisplayName("Naftaleno (mg/Kg)"), Browsable(true)]
        public double Naphthalene { get; set; }

        [DisplayName("Acenafteno (mg/Kg)"), Browsable(true)]
        public double Acenafthene { get; set; }

        [DisplayName("Acenaftileno (mg/Kg)"), Browsable(true)]
        public double Acenaphthylene { get; set; }

        [DisplayName("Fluoreno (mg/Kg)"), Browsable(true)]
        public double Fluorene { get; set; }

        [DisplayName("Antraceno (mg/Kg)"), Browsable(true)]
        public double Anthracene { get; set; }

        [DisplayName("Fenantreno (mg/Kg)"), Browsable(true)]
        public double Fenanthrene { get; set; }

        [DisplayName("Fluoranteno (mg/Kg)"), Browsable(true)]
        public double Fluoranthene { get; set; }

        [DisplayName("Pireno (mg/Kg)"), Browsable(true)]
        public double Pyrene { get; set; }

        [DisplayName("Criseno (mg/Kg)"), Browsable(true)]
        public double Crysene { get; set; }

        [DisplayName("Benzo(a)antraceno (mg/Kg)"), Browsable(true)]
        public double BenzoAAnthracene { get; set; }

        [DisplayName("Benzo(a)pireno (mg/Kg)"), Browsable(true)]
        public double BenzoAPyrene { get; set; }

        [DisplayName("Benzo(b)fluoranteno (mg/Kg)"), Browsable(true)]
        public double BenzoBFluoranthene { get; set; }

        [DisplayName("Benzo(g,h,i)perileno (mg/Kg)"), Browsable(true)]
        public double BenzoGHIPerylene { get; set; }

        [DisplayName("Benzo(k)fluoranteno (mg/Kg)"), Browsable(true)]
        public double BenzoKFluoranthene { get; set; }

        [DisplayName("Dibenzo(a,h)antraceno (mg/Kg)"), Browsable(true)]
        public double DibenzoAHAnthracene { get; set; }

        [DisplayName("Indeno(1,2,3-cd)pireno (mg/Kg)"), Browsable(true)]
        public double Indeno123CDPyrene { get; set; }

        [DisplayName("Arsénico (mg/Kg)"), Browsable(true)]
        public double Arsenic { get; set; }

        [DisplayName("Cadmio (mg/Kg)"), Browsable(true)]
        public double Cadmium { get; set; }

        [DisplayName("Cobre (mg/Kg)"), Browsable(true)]
        public double Copper { get; set; }

        [DisplayName("Cromo total (mg/Kg)"), Browsable(true)]
        public double TotalChrome { get; set; }

        [DisplayName("Mercurio (mg/Kg)"), Browsable(true)]
        public double Mercury { get; set; }

        [DisplayName("Níquel (mg/Kg)"), Browsable(true)]
        public double Nickel { get; set; }

        [DisplayName("Plomo (mg/Kg)"), Browsable(true)]
        public double Lead { get; set; }

        [DisplayName("Selenio (mg/Kg)"), Browsable(true)]
        public double Selenium { get; set; }

        [DisplayName("Zinc (mg/Kg)"), Browsable(true)]
        public double Zinc { get; set; }
        #endregion

        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                return GetBrowsableProperties(typeof(SoilAnalysis));
            }
        }

        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> DoubleProperties
        {
            get
            {
                return GetDoubleBrowsableProperties(typeof(SoilAnalysis));
            }
        }

        public static string GetDisplayName(string propertyName)
        {
            return GetDisplayName(typeof(SoilAnalysis), propertyName);
        }

        public static string GetChemicalAnalysisUnits(string propertyName)
        {
            return SoilAnalysisTypes[propertyName].Unit;
        }
    }
}
