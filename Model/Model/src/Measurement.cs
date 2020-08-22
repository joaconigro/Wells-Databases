using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Wells.BaseModel.Attributes;
using Wells.BaseModel.Models;

namespace Wells.Model
{
    public class Measurement : BusinessObject
    {
        public Measurement() { }

        #region Properties
        [Required]
        [Browsable(false)]
        public string WellId { get; set; }


        [Browsable(true), DisplayName("Pozo")]
        public string WellName => Well?.Name;


        [DisplayName("Fecha"), Browsable(true)]
        public DateTime Date { get; set; }


        [DisplayName("Profundidad Agua (m)"), Browsable(true), NullValue()]
        public double WaterDepth { get; set; }


        [Browsable(false)]
        public bool HasWater => WaterDepth != NumericNullValue;


        [DisplayName("Cota Agua (m)"), Browsable(true), NullValue()]
        public double WaterElevation
        {
            get
            {
                if (HasWater && Well != null)
                {
                    if (Well.HasHeight && Well.HasZ) { return Well.Z + Well.Height - WaterDepth; }
                    else if (Well.HasZ) { return Well.Z - WaterDepth; }
                    return -WaterDepth;
                }
                return NumericNullValue;
            }
        }


        [DisplayName("Caudal"), Browsable(true), NullValue()]
        public double Caudal { get; set; }


        [DisplayName("Observaciones"), Browsable(true)]
        public string Comment { get; set; }
        #endregion


        #region Lazy-Load Properties
        /// <summary>
        /// The parent well.
        /// </summary>
        [ForeignKey("WellId")]
        [Browsable(false)]
        public virtual Well Well { get; set; }

        #endregion


        public override string ToString()
        {
            return Date.ToString(DateFormat);
        }

        public override int CompareTo(IBusinessObject other)
        {
            return Date.CompareTo((other as Measurement).Date);
        }


        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                return GetBrowsableProperties(typeof(Measurement));
            }
        }

        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> DoubleProperties
        {
            get
            {
                return GetDoubleBrowsableProperties(typeof(Measurement));
            }
        }

        public static string GetDisplayName(string propertyName)
        {
            return GetDisplayName(typeof(Measurement), propertyName);
        }
    }
}
