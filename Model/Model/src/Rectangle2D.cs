namespace Wells.Model
{
    public class Rectangle2D
    {
        public double X0;
        public double X1;
        public double Y0;
        public double Y1;

        Rectangle2D(double x0, double x1, double y0, double y1)
        {
            X0 = x0; X1 = x1; Y0 = y0; Y1 = y1;
        }

        public bool Contains(double x, double y)
        {
            if (x < X0) return false;
            if (x > X1) return false;
            if (y < Y0) return false;
            if (y > Y1) return false;
            return true;
        }

        public bool Contains(Well well)
        {
            return Contains(well.X, well.Y);
        }
        
        public static Rectangle2D Torches => new Rectangle2D(0, 4000, 970, 2000);

        public static Rectangle2D ZoneA => new Rectangle2D(0, 1990, 0, 500);

        public static Rectangle2D ZoneB => new Rectangle2D(0, 1990, 500, 970);

        public static Rectangle2D ZoneC => new Rectangle2D(1990, 4000, 500, 970);

        public static Rectangle2D ZoneD => new Rectangle2D(1990, 4000, 0, 500);
    }
}