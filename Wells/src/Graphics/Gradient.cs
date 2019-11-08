using System.IO;
using System.Linq;
using System.Windows.Markup;
using System.Windows.Media;
using System.Xml.Serialization;
using Wells.Base;

namespace Wells.View.Graphics
{
    public class Gradient
    {
        public Gradient() {
            Name = "Nuevo";
            LinearGradient = new LinearGradientBrush(Colors.White, Colors.Black, 0.0);
        }

        public Gradient(string name, LinearGradientBrush linearGradientBrush)
        {
            Name = name;
            LinearGradient = linearGradientBrush;
        }

        public string Name { get; set; }

        [XmlIgnore]
        public LinearGradientBrush LinearGradient { get; set; }

        public string LinearGradientString { get; set; }

        public void SerializeGradient()
        {
            using (var stream = new StringWriter())
            {
                XamlWriter.Save(LinearGradient, stream);
                LinearGradientString = stream.ToString();
            }
        }

        public void DeserializeGradient()
        {
            using (var stream = LinearGradientString.GetStream())
            {
                LinearGradient = (LinearGradientBrush)XamlReader.Load(stream);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public void Invert()
        {
            var stops = LinearGradient.GradientStops.OrderBy(x => x.Offset).ToList();
            foreach(var s in stops)
            {
                s.Offset = (s.Offset - 1.0) * -1.0;
            }
            stops = stops.OrderBy(x => x.Offset).ToList();
            var grad = new GradientStopCollection(stops);
            LinearGradient = new LinearGradientBrush(grad);
        }

        public Color GetColor(double offset)
        {
            var stops = LinearGradient.GradientStops.OrderBy(x => x.Offset).ToList();

            if (offset <= 0 || double.IsNaN(offset))
            {
                return stops[0].Color;
            }
            if (offset >= 1 || double.IsInfinity(offset))
            {
                return stops.Last().Color;
            }
            if (stops.Exists(s => s.Offset.Equals(offset)))
            {
                return stops.Find(s => s.Offset.Equals(offset)).Color;
            }

            int i = 1;
            while (i < stops.Count && stops[i].Offset < offset)
            {
                i++;
            }
            var left = stops[i - 1];
            var right = stops[i];

            offset = (offset - left.Offset) / (right.Offset - left.Offset);
            float a = (float)((right.Color.ScA - left.Color.ScA) * offset + left.Color.ScA);
            float r = (float)((right.Color.ScR - left.Color.ScR) * offset + left.Color.ScR);
            float g = (float)((right.Color.ScG - left.Color.ScG) * offset + left.Color.ScG);
            float b = (float)((right.Color.ScB - left.Color.ScB) * offset + left.Color.ScB);
            return Color.FromScRgb(a, r, g, b);
        }
    }
}
