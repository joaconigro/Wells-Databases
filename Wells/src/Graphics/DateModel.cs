using System;

namespace Wells.View.Graphics
{
    public class DateModel
    {
        public DateTime SampleDate { get; set; }
        public double X { get; set; }
        public double Y { get; set; }

        public DateModel(DateTime sampleDate, double y)
        {
            SampleDate = sampleDate;
            Y = y;
        }

        public DateModel(DateTime sampleDate, double x, double y)
        {
            SampleDate = sampleDate;
            X = x;
            Y = y;
        }
    }
}
