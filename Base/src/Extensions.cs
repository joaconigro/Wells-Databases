using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Wells.Base
{
    public static class Extensions
    {

        public static double StandardDeviation(this IEnumerable<double> source)
        {
            var average = source.Average();
            var sum = Math.Pow(source.Sum(v => v - average), 2.0);
            return Math.Sqrt(sum / source.Count());
        }

        public static double StandardDeviation<T>(this IEnumerable<T> source, Func<T, double> selector)
        {
            var list = source.Select(selector);
            return StandardDeviation(list);
        }

        public static MemoryStream GetStream(this string value)
        {
            return value.GetStream(Encoding.UTF8);
        }

        public static MemoryStream GetStream(this string value, Encoding encoding)
        {
            return new MemoryStream(encoding.GetBytes(value ?? ""));
        }
    }
}
