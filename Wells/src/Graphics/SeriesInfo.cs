using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Xml.Serialization;
using Wells.Model;

namespace Wells.View.Graphics
{
    public class SeriesInfo
    {
        private Color color;

        public bool IsFromWell { get; set; }
        public Well Well { get; }
        public string PropertyXName { get; }
        public string DisplayXName { get; set; }
        public string PropertyYName { get; }
        public string DisplayYName { get; set; }
        public string ListName { get; set; }
        public bool IsDateBased { get; set; }
        public Func<Well, string, string, string, string, List<DateModel>> GetValuesFunc { get; }
        public string HexColor { get; set; }

        [XmlIgnore]
        public Color Color
        {
            get
            {
                DeserializeColor();
                return color;
            }
            set
            {
                color = value;
                HexColor = string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B);
            }
        }

        void DeserializeColor()
        {
            if (string.IsNullOrEmpty(HexColor))
            {
                color = Colors.Black;
            }
            else
            {
                var a = byte.Parse(HexColor.Substring(1, 2), System.Globalization.NumberStyles.HexNumber);
                var r = byte.Parse(HexColor.Substring(3, 2), System.Globalization.NumberStyles.HexNumber);
                var g = byte.Parse(HexColor.Substring(5, 2), System.Globalization.NumberStyles.HexNumber);
                var b = byte.Parse(HexColor.Substring(7, 2), System.Globalization.NumberStyles.HexNumber);
                color = Color.FromArgb(a, r, g, b);
            }
        }

        public SeriesInfo() { }

        public SeriesInfo(bool isFromWell, string listName, string displayXName, string displayYName)
        {
            IsFromWell = isFromWell;
            ListName = listName;
            DisplayXName = displayXName;
            DisplayYName = displayYName;
            IsDateBased = displayXName == "Fecha";
        }

        public SeriesInfo(bool isFromWell)
        {
            IsFromWell = isFromWell;
            Well = null;
            ListName = "Precipitaciones";
            PropertyXName = string.Empty;
            DisplayXName = string.Empty;
            PropertyYName = string.Empty;
            DisplayYName = string.Empty;
            GetValuesFunc = null;
            IsDateBased = true;
        }


        public SeriesInfo(Well well, string listName, string propertyXName, string displayXName, string propertyYName, string displayYName,
               Func<Well, string, string, string, string, List<DateModel>> func)
        {
            Well = well;
            ListName = listName;
            PropertyXName = propertyXName;
            DisplayXName = displayXName;
            PropertyYName = propertyYName;
            DisplayYName = displayYName;
            GetValuesFunc = func;
            IsDateBased = displayXName == "Fecha";
        }

        public List<DateModel> GetValues()
        {
            return GetValuesFunc?.Invoke(Well, ListName, PropertyXName, PropertyYName, DisplayYName);
        }

        public override string ToString()
        {
            if (IsFromWell)
            {
                if (Well != null)
                {
                    return $"{Well.Name} - {DisplayYName} vs {DisplayXName}";
                }
                else
                {
                    return $"{DisplayYName} vs {DisplayXName}";
                }
            }
            return "Precipitaciones";
        }

    }
}
