using System;

namespace Wells.View.Graphics
{
    public class DateModel
    {
        public DateTime SampleDate { get; set; }
        public double Value { get; set; }

        public DateModel(DateTime sampleDate, double value)
        {
            SampleDate = sampleDate;
            Value = value;
        }
    }
}
