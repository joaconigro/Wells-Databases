using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Wells.StandardModel.Models;

namespace Wells.YPFModel
{
    public class Precipitation : BusinessObject
    {

        public Precipitation() : base() { }
        
        #region Properties
        [Required, DisplayName("Fecha"), Browsable(true)]
        public DateTime PrecipitationDate { get; set; }

        [Required, DisplayName("Milímetros"), Browsable(true)]
        public double Millimeters { get; set; }
        #endregion

        [Browsable(false)]
        public static Dictionary<string, PropertyInfo> Properties
        {
            get
            {
                return GetBrowsableProperties(typeof(Precipitation));
            }
        }

        public override int CompareTo(IBusinessObject other)
        {
            return PrecipitationDate.CompareTo((other as Precipitation).PrecipitationDate);
        }

        public static string GetDisplayName(string propertyName)
        {
            return GetDisplayName(typeof(Precipitation), propertyName);
        }
    }
}