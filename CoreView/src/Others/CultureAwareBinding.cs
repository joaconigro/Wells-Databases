using System;
using System.Windows.Data;
using System.Globalization;

namespace Wells.CoreView
{
    public class CultureAwareBinding : Binding
    {
        public CultureAwareBinding() : base() { ConverterCulture = CultureInfo.CurrentCulture; }

        public CultureAwareBinding(string path) : base(path) { ConverterCulture = CultureInfo.CurrentCulture; }
    }
}
