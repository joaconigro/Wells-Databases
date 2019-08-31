using System;
using System.Collections.Generic;
using System.ComponentModel;
using Wells.StandardModel.Models;
using System.Linq;

namespace Wells.YPFModel
{
    public class Well : BusinessObject
    {
        public Well() : base() { }

        public Well(string name) : base(name) { }

        #region Properties
        [DisplayName("X"), Browsable(true)]
        public double X { get; set; }

        [DisplayName("Y"), Browsable(true)]
        public double Y { get; set; }

        [DisplayName("Cota"), Browsable(true)]
        public double Z { get; set; }

        [DisplayName("Latitud"), Browsable(true)]
        public double Latitude { get; set; }

        [DisplayName("Longitud"), Browsable(true)]
        public double Longitude { get; set; }

        [DisplayName("Altura"), Browsable(true)]
        public double Height { get; set; }

        [DisplayName("Fondo"), Browsable(true)]
        public double Bottom { get; set; }

        [Browsable(false)]
        public bool HasGeographic => !(Latitude == NumericNullValue && Longitude == NumericNullValue);

        [Browsable(false)]
        public bool HasHeight => Height != NumericNullValue;

        [DisplayName("Tipo"), Browsable(true)]
        public WellType WellType { get; set; }


        [DisplayName("Existe"), Browsable(true)]
        public bool Exists { get; set; }


        [Browsable(false)]
        public List<SoilAnalysis> SoilAnalyses { get
            {
                return Analyses?.Where((a) => a.SampleOf == SampleType.Soil).Select((a) => a as SoilAnalysis).ToList();
            } }

        [Browsable(false)]
        public List<WaterAnalysis> WaterAnalyses
        {
            get
            {
                return Analyses?.Where((a) => a.SampleOf == SampleType.Water).Select((a) => a as WaterAnalysis).ToList();
            }
        }

        [Browsable(false)]
        public List<FLNAAnalysis> FLNAAnalyses
        {
            get
            {
                return Analyses?.Where((a) => a.SampleOf == SampleType.FLNA).Select((a) => a as FLNAAnalysis).ToList();
            }
        }


        #endregion


        #region Lazy-loaded properties
        [Browsable(false)]
        public virtual List<ChemicalAnalysis> Analyses { get; set; }

        [Browsable(false)]
        public virtual List<Measurement> Measurements { get; set; }

        [Browsable(false)]
        public virtual List<ExternalFile> Files { get; set; }
#endregion
    }
}