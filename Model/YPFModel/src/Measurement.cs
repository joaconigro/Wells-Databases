using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Wells.BaseModel.Models;

namespace Wells.YPFModel
{
    public class Measurement : BusinessObject
    {


        #region Properties
        [Required]
        [Browsable(false)]
        public string WellId { get; set; }


        [Browsable(true), DisplayName("Pozo")]
        public string WellName => Well?.Name;


        [DisplayName("Fecha"), Browsable(true)]
        public DateTime Date { get; set; }


        [DisplayName("Profundidad FLNA"), Browsable(true)]
        public double FLNADepth { get; set; }


        [DisplayName("Profundidad Agua"), Browsable(true)]
        public double WaterDepth { get; set; }


        [Browsable(false)]
        public bool HasWater => WaterDepth != NumericNullValue;


        [Browsable(false)]
        public bool HasFLNA => FLNADepth != NumericNullValue;

        [DisplayName("Espesor FLNA"), Browsable(true)]
        public double FLNAThickness
        {
            get
            {
                if (HasFLNA && HasWater) return WaterDepth - FLNADepth;
                return NumericNullValue;
            }
        }


        [DisplayName("Cota FLNA"), Browsable(true)]
        public double FLNAElevation
        {
            get
            {
                if (HasFLNA && Well != null)
                {
                    if (Well.HasHeight) return Well.Z + Well.Height - FLNADepth;
                    return Well.Z - FLNADepth;
                }
                return NumericNullValue;
            }
        }


        [DisplayName("Cota Agua"), Browsable(true)]
        public double WaterElevation
        {
            get
            {
                if (HasWater && Well != null)
                {
                    if (Well.HasHeight) return Well.Z + Well.Height - WaterDepth;
                    return Well.Z - WaterDepth;
                }
                return NumericNullValue;
            }
        }

        [DisplayName("Caudal"), Browsable(true)]
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
            if (Date > (other as Measurement).Date) return -1;
            if (Date == (other as Measurement).Date) return 0;
            return 1;
        }
    }
}
