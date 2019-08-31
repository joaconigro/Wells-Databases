using System.ComponentModel;
using Wells.StandardModel.Models;

namespace Wells.YPFModel
{
    public class SoilAnalysis : ChemicalAnalysis
    {
        public SoilAnalysis() : base() { SampleOf = SampleType.Soil; }

        public SoilAnalysis(Well well) : base(well) { SampleOf = SampleType.Soil; }

        public override ChemicalAnalysisType GetChemicalAnalysisType(string propertyName)
        {
            return SoilAnalysisTypes[propertyName];
        }


        #region Properties
        [DisplayName("Humedad"), Browsable(true)]
        public double Humidity { get; set; }

        [DisplayName("pH"), Browsable(true)]
        public double PH { get; set; }

        [DisplayName("GRO"), Browsable(true)]
        public double GRO { get; set; }

        [DisplayName("DRO"), Browsable(true)]
        public double DRO { get; set; }

        [DisplayName("MRO"), Browsable(true)]
        public double MRO { get; set; }

        [DisplayName("Hidrocarburos totales (EPA 8015)"), Browsable(true)]
        public double TotalHydrocarbons_EPA8015 { get; set; }

        [DisplayName("Hidrocarburos totales (TNRCC 1005)"), Browsable(true)]
        public double TotalHydrocarbons_TNRCC1005 { get; set; }

        [DisplayName("Aceites y grasas"), Browsable(true)]
        public double OilsAndFats { get; set; }

        [DisplayName("> C6 - C8 (F. alifática)"), Browsable(true)]
        public double C6_C8Aliphatic { get; set; }

        [DisplayName("> C8 - C10 (F. alifática)"), Browsable(true)]
        public double C8_C10Aliphatic { get; set; }

        [DisplayName("> C10 - C12 (F. alifática)"), Browsable(true)]
        public double C10_C12Aliphatic { get; set; }

        [DisplayName("> C12 - C16 (F. alifática)"), Browsable(true)]
        public double C12_C16Aliphatic { get; set; }

        [DisplayName("> C16 - C21 (F. alifática)"), Browsable(true)]
        public double C16_C21Aliphatic { get; set; }

        [DisplayName("> C21 - C35 (F. alifática)"), Browsable(true)]
        public double C21_C35Aliphatic { get; set; }

        [DisplayName("> C6 - C8 (F. aromática)"), Browsable(true)]
        public double C6_C8Aromatic { get; set; }

        [DisplayName("> C8 - C10 (F. aromática)"), Browsable(true)]
        public double C8_C10Aromatic { get; set; }

        [DisplayName("> C10 - C12 (F. aromática)"), Browsable(true)]
        public double C10_C12Aromatic { get; set; }

        [DisplayName("> C12 - C16 (F. aromática)"), Browsable(true)]
        public double C12_C16Aromatic { get; set; }

        [DisplayName("> C16 - C21 (F. aromática)"), Browsable(true)]
        public double C16_C21Aromatic { get; set; }

        [DisplayName("> C21 - C35 (F. aromática)"), Browsable(true)]
        public double C21_C35Aromatic { get; set; }

        [DisplayName("Benceno"), Browsable(true)]
        public double Benzene { get; set; }

        [DisplayName("Tolueno"), Browsable(true)]
        public double Tolueno { get; set; }

        [DisplayName("Etilbenceno"), Browsable(true)]
        public double Ethylbenzene { get; set; }

        [DisplayName("Xileno (o)"), Browsable(true)]
        public double XyleneO { get; set; }

        [DisplayName("Xileno (p-m)"), Browsable(true)]
        public double XylenePM { get; set; }

        [DisplayName("Xileno total"), Browsable(true)]
        public double TotalXylene { get; set; }

        [DisplayName("Naftaleno"), Browsable(true)]
        public double Naphthalene { get; set; }

        [DisplayName("Acenafteno"), Browsable(true)]
        public double Acenafthene { get; set; }

        [DisplayName("Acenaftileno"), Browsable(true)]
        public double Acenaphthylene { get; set; }

        [DisplayName("Fluoreno"), Browsable(true)]
        public double Fluorene { get; set; }

        [DisplayName("Antraceno"), Browsable(true)]
        public double Anthracene { get; set; }

        [DisplayName("Fenantreno"), Browsable(true)]
        public double Fenanthrene { get; set; }

        [DisplayName("Fluoranteno"), Browsable(true)]
        public double Fluoranthene { get; set; }

        [DisplayName("Pireno"), Browsable(true)]
        public double Pyrene { get; set; }

        [DisplayName("Criseno"), Browsable(true)]
        public double Crysene { get; set; }

        [DisplayName("Benzo(a)antraceno"), Browsable(true)]
        public double BenzoAAnthracene { get; set; }

        [DisplayName("Benzo(a)pireno"), Browsable(true)]
        public double BenzoAPyrene { get; set; }

        [DisplayName("Benzo(b)fluoranteno"), Browsable(true)]
        public double BenzoBFluoranthene { get; set; }

        [DisplayName("Benzo(g,h,i)perileno"), Browsable(true)]
        public double BenzoGHIPerylene { get; set; }

        [DisplayName("Benzo(k)fluoranteno"), Browsable(true)]
        public double BenzoKFluoranthene { get; set; }

        [DisplayName("Dibenzo(a,h)antraceno"), Browsable(true)]
        public double DibenzoAHAnthracene { get; set; }

        [DisplayName("Indeno(1,2,3-cd)pireno"), Browsable(true)]
        public double Indeno123CDPyrene { get; set; }

        [DisplayName("Arsénico"), Browsable(true)]
        public double Arsenic { get; set; }

        [DisplayName("Cadmio"), Browsable(true)]
        public double Cadmium { get; set; }

        [DisplayName("Cobre"), Browsable(true)]
        public double Copper { get; set; }

        [DisplayName("Cromo total"), Browsable(true)]
        public double TotalChrome { get; set; }

        [DisplayName("Mercurio"), Browsable(true)]
        public double Mercury { get; set; }

        [DisplayName("Níquel"), Browsable(true)]
        public double Nickel { get; set; }

        [DisplayName("Plomo"), Browsable(true)]
        public double Lead { get; set; }

        [DisplayName("Selenio"), Browsable(true)]
        public double Selenium { get; set; }

        [DisplayName("Zinc"), Browsable(true)]
        public double Zinc { get; set; }
        #endregion
    }
}
