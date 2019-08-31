using System;

namespace Wells.StandardModel.Attributes
{
    /// <summary>
    /// An attribute used to define column index used to display in a DataGrid control.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class SortIndexAttribute : Attribute
    {

        /// Initializes a new instance of the <see cref="SortIndexAttribute" /> class.
        /// <param name="index">The column index used to display in a DataGrid control.</param>
        public SortIndexAttribute(uint index)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(Index));
            }

            Index = index;
        }

        public uint Index { get; set; }
    }
}
