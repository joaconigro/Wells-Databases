﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Wells.BaseModel.Attributes;

namespace Wells.BaseModel.Models
{
    public abstract class BusinessObject : IBusinessObject, IComparable<IBusinessObject>
    {
        public static readonly double NumericNullValue = -9999.0;

        public static readonly string DateFormat = "dd/MM/yyyy";

        #region Constructors
        protected BusinessObject()
        {
            Id = Guid.NewGuid().ToString();
        }
             
        #endregion

        #region IBusinessObject implementation

        [Browsable(false), Required]
        public string Id { get; set; }


        public static Dictionary<string, PropertyInfo> GetBrowsableProperties(Type type)
        {
            var dict = (from p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                        let a = p.GetCustomAttribute(typeof(BrowsableAttribute)) as BrowsableAttribute
                        where (a != null && a.Browsable)
                        select ((p.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute).DisplayName, p)).ToDictionary(o => o.DisplayName, o => o.p);

            return dict;
        }


        public static string GetDisplayName(Type type, string propertyName) {
            var display = (from p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                        let a = p.GetCustomAttribute(typeof(BrowsableAttribute)) as BrowsableAttribute
                        where (a != null && a.Browsable && p.Name == propertyName)
                        select ((p.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute).DisplayName)).FirstOrDefault();
            return display;
        }

        protected static Dictionary<string, PropertyInfo> GetBrowsablePropertiesOfType(Type type, Type propertyType)
        {
            var dict = GetBrowsableProperties(type).Where(kp => kp.Value.PropertyType == propertyType)
                       .ToDictionary(kp => kp.Key, kp => kp.Value);
            return dict;
        }

        public static Dictionary<string, PropertyInfo> GetDoubleBrowsableProperties(Type type)
        {
            var dict = GetBrowsablePropertiesOfType(type, typeof(double));
            return dict;
        }

        #endregion

        public abstract int CompareTo(IBusinessObject other);       
    }

    public enum WellType
    {
        [Description("Pozo")] MeasurementWell,
        [Description("Sondeo")] Sounding
    }
}
