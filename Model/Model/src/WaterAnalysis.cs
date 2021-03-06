﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Wells.BaseModel.Models;

namespace Wells.Model
{
    public class WaterAnalysis : ChemicalAnalysis
    {
        public WaterAnalysis() { }

        public WaterAnalysis(Well well) : base(well) { }

        public override ChemicalAnalysisType GetChemicalAnalysisType(string propertyName)
        {
            return WaterAnalysisTypes[propertyName.ToLowerInvariant()];
        }

        #region Properties
        [DisplayName("pH (UpH)"), Browsable(true)]
        public double Ph { get; set; }

        [DisplayName("Conductividad (uS/cm)"), Browsable(true)]
        public double Conductivity { get; set; }

        [DisplayName("Residuos Secos (mg/l)"), Browsable(true)]
        public double DryWaste { get; set; }

        [DisplayName("Alcalinidad de Bicarbonato (mg/l)"), Browsable(true)]
        public double BicarbonateAlkalinity { get; set; }

        [DisplayName("Alcalinidad de Carbonato (mg/l)"), Browsable(true)]
        public double CarbonateAlkalinity { get; set; }

        [DisplayName("Cloruros (mg/l)"), Browsable(true)]
        public double Chlorides { get; set; }

        [DisplayName("Nitratos (mg/l)"), Browsable(true)]
        public double Nitrates { get; set; }

        [DisplayName("Sulfatos (mg/l)"), Browsable(true)]
        public double Sulfates { get; set; }

        [DisplayName("Calcio (mg/l)"), Browsable(true)]
        public double Calcium { get; set; }

        [DisplayName("Magnesio (mg/l)"), Browsable(true)]
        public double Magnesium { get; set; }

        [DisplayName("Sulfuros Totales (HS-) (mg/l)"), Browsable(true)]
        public double TotalSulfur { get; set; }

        [DisplayName("Potasio (mg/l)"), Browsable(true)]
        public double Potassium { get; set; }

        [DisplayName("Sodio (mg/l)"), Browsable(true)]
        public double Sodium { get; set; }

        [DisplayName("Fluoruros (mg/l)"), Browsable(true)]
        public double Fluorides { get; set; }

        [DisplayName("DRO (mg/l)"), Browsable(true)]
        public double Dro { get; set; }

        [DisplayName("GRO (mg/l)"), Browsable(true)]
        public double Gro { get; set; }

        [DisplayName("MRO (mg/l)"), Browsable(true)]
        public double Mro { get; set; }

        [DisplayName("Hidrocarburos totales (EPA 8015) (mg/l)"), Browsable(true)]
        public double TotalHydrocarbonsEpa8015 { get; set; }

        [DisplayName("Hidrocarburos totales (TNRCC 1005) (mg/l)"), Browsable(true)]
        public double TotalHydrocarbonsTnrcc1005 { get; set; }

        [DisplayName("Benceno (mg/l)"), Browsable(true)]
        public double Benzene { get; set; }

        [DisplayName("Tolueno (mg/l)"), Browsable(true)]
        public double Tolueno { get; set; }

        [DisplayName("Etilbenceno (mg/l)"), Browsable(true)]
        public double Ethylbenzene { get; set; }

        [DisplayName("Xileno (o) (mg/l)"), Browsable(true)]
        public double XyleneO { get; set; }

        [DisplayName("Xileno (p-m) (mg/l)"), Browsable(true)]
        public double XylenePm { get; set; }

        [DisplayName("Xileno total (mg/l)"), Browsable(true)]
        public double TotalXylene { get; set; }

        [DisplayName("Naftaleno (mg/l)"), Browsable(true)]
        public double Naphthalene { get; set; }

        [DisplayName("Acenafteno (mg/l)"), Browsable(true)]
        public double Acenafthene { get; set; }

        [DisplayName("Acenaftileno (mg/l)"), Browsable(true)]
        public double Acenaphthylene { get; set; }

        [DisplayName("Fluoreno (mg/l)"), Browsable(true)]
        public double Fluorene { get; set; }

        [DisplayName("Antraceno (mg/l)"), Browsable(true)]
        public double Anthracene { get; set; }

        [DisplayName("Fenantreno (mg/l)"), Browsable(true)]
        public double Fenanthrene { get; set; }

        [DisplayName("Fluoranteno (mg/l)"), Browsable(true)]
        public double Fluoranthene { get; set; }

        [DisplayName("Pireno (mg/l)"), Browsable(true)]
        public double Pyrene { get; set; }

        [DisplayName("Benzo(a)antraceno (mg/l)"), Browsable(true)]
        public double BenzoAAnthracene { get; set; }

        [DisplayName("Criseno (mg/l)"), Browsable(true)]
        public double Crysene { get; set; }

        [DisplayName("Benzo(a)pireno (mg/l)"), Browsable(true)]
        public double BenzoAPyrene { get; set; }

        [DisplayName("Benzo(b)fluoranteno (mg/l)"), Browsable(true)]
        public double BenzoBFluoranthene { get; set; }

        [DisplayName("Benzo(g,h,i)perileno (mg/l)"), Browsable(true)]
        public double BenzoGhiPerylene { get; set; }

        [DisplayName("Benzo(k)fluoranteno (mg/l)"), Browsable(true)]
        public double BenzoKFluoranthene { get; set; }

        [DisplayName("Dibenzo(a,h)antraceno (mg/l)"), Browsable(true)]
        public double DibenzoAhAnthracene { get; set; }

        [DisplayName("Indeno(1,2,3-cd)pireno (mg/l)"), Browsable(true)]
        public double Indeno123CdPyrene { get; set; }

        [DisplayName("Arsénico (mg/l)"), Browsable(true)]
        public double Arsenic { get; set; }

        [DisplayName("Cadmio (mg/l)"), Browsable(true)]
        public double Cadmium { get; set; }

        [DisplayName("Cobalto (mg/l)"), Browsable(true)]
        public double Cobalt { get; set; }

        [DisplayName("Cobre (mg/l)"), Browsable(true)]
        public double Copper { get; set; }

        [DisplayName("Cromo total (mg/l)"), Browsable(true)]
        public double TotalChrome { get; set; }

        [DisplayName("Mercurio (mg/l)"), Browsable(true)]
        public double Mercury { get; set; }

        [DisplayName("Níquel (mg/l)"), Browsable(true)]
        public double Nickel { get; set; }

        [DisplayName("Plomo (mg/l)"), Browsable(true)]
        public double Lead { get; set; }

        [DisplayName("Zinc (mg/l)"), Browsable(true)]
        public double Zinc { get; set; }
        #endregion


        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                return GetBrowsableProperties(typeof(WaterAnalysis));
            }
        }

        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> DoubleProperties
        {
            get
            {
                return GetDoubleBrowsableProperties(typeof(WaterAnalysis));
            }
        }

        public static string GetDisplayName(string propertyName)
        {
            return GetDisplayName(typeof(WaterAnalysis), propertyName);
        }

        public static string GetChemicalAnalysisUnits(string propertyName)
        {
            return WaterAnalysisTypes[propertyName.ToLowerInvariant()].Unit;
        }
    }
}
