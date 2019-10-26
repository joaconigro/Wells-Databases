using System;
using System.Collections.Generic;
using System.Text;

namespace Wells.View.Graphics
{
    public class PremadeSeriesInfo
    {
        public string ParameterGroup { get; set; }
        public string PropertyDisplayName { get; set; }
        public bool IsFromWell { get; set; }

        public override string ToString()
        {
            if (IsFromWell) return PropertyDisplayName;
            return "Precipitaciones";
        }
      
    }
}
