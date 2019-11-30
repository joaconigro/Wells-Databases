﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using Wells.BaseModel.Models;

namespace Wells.Model
{
    public class Well : BusinessObject
    {
        public Well()
        {
            WaterAnalyses = new List<WaterAnalysis>();
            Measurements = new List<Measurement>();
            Files = new List<ExternalFile>();
        }

        [Browsable(true), DisplayName("Nombre")]
        public string Name { get; set; }

        #region Properties
        [DisplayName("Latitud"), Browsable(true)]
        public double Latitude { get; set; }

        [DisplayName("Longitud"), Browsable(true)]
        public double Longitude { get; set; }

        [DisplayName("Cota (m.s.n.m.)"), Browsable(true)]
        public double Z { get; set; }        

        [DisplayName("Altura del brocal (m)"), Browsable(true)]
        public double Height { get; set; }

        [DisplayName("Fondo (m.b.b.p.)"), Browsable(true)]
        public double Bottom { get; set; }

        [Browsable(false)]
        public bool HasGeographic => !(Latitude.Equals(NumericNullValue) || Longitude.Equals(NumericNullValue));

        [Browsable(false)]
        public bool HasHeight => !Height.Equals(NumericNullValue);

        [Browsable(false)]
        public bool HasZ => !Z.Equals(NumericNullValue);

        [DisplayName("Existe"), Browsable(true)]
        public bool Exists { get; set; }

        #endregion


        #region Lazy-loaded properties
        [Browsable(false)]
        public virtual List<WaterAnalysis> WaterAnalyses { get; set; }

        [Browsable(false)]
        public virtual List<Measurement> Measurements { get; set; }

        [Browsable(false)]
        public virtual List<ExternalFile> Files { get; set; }
        #endregion



        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                return GetBrowsableProperties(typeof(Well));
            }
        }

        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> DoubleProperties
        {
            get
            {
                return GetDoubleBrowsableProperties(typeof(Well));
            }
        }

        public override int CompareTo(IBusinessObject other)
        {
            return Name.CompareTo((other as Well).Name);
        }

        public static string GetDisplayName(string propertyName)
        {
            return GetDisplayName(typeof(Well), propertyName);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}