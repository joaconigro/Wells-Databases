using System;
using Wells.BaseModel.Models;

namespace Wells.BaseModel.Attributes
{
    /// <summary>
    /// An attribute used to define column index used to display in a DataGrid control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class NullValueAttribute : Attribute
    {

        /// Initializes a new instance of the <see cref="NullValueAttribute" /> class.
        /// <param name="index">The column index used to display in a DataGrid control.</param>
        public NullValueAttribute(string nullValue)
        {
            NullValue = nullValue;
        }

        public NullValueAttribute()
        {
            NullValue = "n/d";
        }

        public string NullValue { get; }
    }
}
