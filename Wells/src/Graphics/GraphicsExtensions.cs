using LiveCharts.Wpf;
using System.Windows.Media;

namespace Wells.View.Graphics
{
    public static class GraphicsExtensions
    {
        public static System.Drawing.Color ToDrawingColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static System.Windows.Media.Color ToMediaColor(this System.Drawing.Color color)
        {
            return System.Windows.Media.Color.FromArgb(color.A, color.R, color.G, color.B);
        }

        public static void SetColor(this LineSeries series, Color color)
        {
            series.Stroke = new SolidColorBrush(color);
        }

        public static void SetColor(this ColumnSeries series, Color color)
        {
            series.Fill = new SolidColorBrush(color);
        }
    }
}
