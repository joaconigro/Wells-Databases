using System;
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

        protected Dictionary<string, PropertyInfo> GetPropertyNames(Type type)
        {
            var list = (from p in type.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.FlattenHierarchy)
                        let a = p.GetCustomAttribute(typeof(BrowsableAttribute)) as BrowsableAttribute
                        where (a != null && a.Browsable)
                        select ((p.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute).DisplayName, p)).ToDictionary(o => o.DisplayName, o => o.p);

            return list;
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
        [DisplayName("Pozo")] MeasurementWell,
        [DisplayName("Sondeo")] Sounding
    }

    public enum SampleType
    {
        [DisplayName("Agua")] Water,
        [DisplayName("FLNA")] FLNA,
        [DisplayName("Suelo")] Soil
    }
}
