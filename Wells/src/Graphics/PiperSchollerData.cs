using System;
using System.Windows;

namespace Wells.View.Graphics
{
    public class PiperSchollerData
    {
        public double Calcium { get; set; }
        public double PotassiumAndSodium { get; set; }
        public double Magnesium { get; set; }
        public double Carbonates { get; set; }
        public double Sulfates { get; set; }
        public double Chlorides { get; set; }
        public string Label { get; set; }
        public System.Windows.Media.Color PointColor { get; set; }
        public bool IsVisible { get; set; }

        public Point Cation { get; private set; }
        public Point Anion { get; private set; }
        public Point Diamond { get; private set; }

        public void CalculateXYPositions(Point lowerLeftCations, Point lowerLeftAnions, double triangleSideLength)
        {
            var scale = triangleSideLength / 100.0;
            Cation = new Point(lowerLeftCations.X + triangleSideLength - (Calcium + Magnesium / 2) * scale,
                               lowerLeftCations.Y - (Math.Sqrt(3.0) * Magnesium / 2) * scale);

            Anion = new Point(lowerLeftAnions.X + (Chlorides + Sulfates / 2) * scale,
                              lowerLeftAnions.Y - (Math.Sqrt(3.0) * Sulfates / 2) * scale);

            Diamond = new Point(0.5 * (Cation.X + Anion.X - (Anion.Y - Cation.Y) / Math.Sqrt(3.0)),
                                0.5 * (Anion.Y + Cation.Y - Math.Sqrt(3.0) * (Anion.X - Cation.X)));
        }

        public override string ToString()
        {
            return Label;
        }
    }
}
