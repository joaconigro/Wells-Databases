using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Wells.StandardModel.Attributes;

namespace Wells.StandardModel.Models
{
    public abstract class BusinessObject : IBusinessObject, IComparable<IBusinessObject>
    {
        public static double NumericNullValue = -9999.0;

        public static string DateFormat = "dd/MM/yyyy";

        #region Constructors
        public BusinessObject()
        {
            Id = Guid.NewGuid().ToString();
        }

        public BusinessObject(string name) :
            this()
        {
            Name = name;
        }
        #endregion

        #region IBusinessObject implementation

        [Browsable(false), Required]
        public string Id { get; set; }

        [Browsable(true), DisplayName("Nombre"), SortIndex(0)]
        public string Name { get; set; }


        public virtual void OnInitialize() { }

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
     
        #endregion

        public override string ToString()
        {
            return Name;
        }

        public virtual int CompareTo(IBusinessObject other)
        {
            return Name.CompareTo(other.Name);
        }
    }

    public enum WellType
    {
        [Description("Pozo")] MeasurementWell,
        [Description("Sondeo")] Sounding
    }

    public enum SampleType
    {
        [Description("Agua")] Water,
        [Description("FLNA")] FLNA,
        [Description("Suelo")] Soil
    }
}
